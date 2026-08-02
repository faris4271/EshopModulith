---
paths:
  - "Src/**/Data/**"
  - "Src/**/Domain/**"
  - "Src/**/Models/**"
  - "Src/**/Configurations/**"
---

# Persistence Rules

EF Core 10 (Npgsql) patterns and repository usage in EshopModulith.

## Repository Reality (read this first)

The previous version of this document described abstractions that **do not exist in the codebase**: there is no `BaseDbContext`, no `BaseEntity`, no `ISoftDelete` interface (the real audit contract is `IAuditableEntity`, not `IAuditable`), no `Specification<T>`, and no separate `Migrations.PostgreSQL` playground project. This file documents what is actually there.

- **DbContext base class is plain `DbContext`** (or ASP.NET `IdentityDbContext<,,>` in the Identity module). There is no shared `BaseDbContext` to inherit from.
- **Audit timestamps and domain-event dispatch are provided by EF Core `SaveChangesInterceptor`s**, not by a base DbContext. Register them in the module's `Add{Module}` (see [modules.md](modules.md)).
- **Migrations live inside each module**, at `{Impl}/Data/Migrations/`, and are auto-applied on startup via `app.UseMigration<{X}DbContext>()`.

## DbContext Pattern — one per module

```csharp
// CatalogModule — plain DbContext
namespace Catalog.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category.Models.Category> Categories { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }   // from Shared.Eventing

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");            // module-specific schema
        // any inline entity config goes here...
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogModule).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
```

The Identity module uses ASP.NET Core Identity, so its DbContext derives from the Identity base instead:

```csharp
namespace IdentityModule.Data;

public class IdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Group> Groups => Set<Group>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("identity");
        modelBuilder.Entity<AppUser>().ToTable("AspNetUsers", "identity");
        modelBuilder.Entity<AppRole>().ToTable("AspNetRoles", "identity");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}
```

### Registration (per module `Add{Module}`)

```csharp
services.AddDbContext<CatalogDbContext>((sp, opt) =>
{
    opt.AddInterceptors(sp.GetRequiredService<DispachDomainEventInterceptor>())
       .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>())
       .UseNpgsql(configuration.GetConnectionString("defualt")); // key is "defualt" (sic) in this repo
});
```

## Audit & Domain Events — interceptors

Two `SaveChangesInterceptor`s from `Shared.Data.interceptores` provide cross-cutting behavior:

| Interceptor | Namespace | Provides |
|---|---|---|
| `AuditableEntityInterceptor` | `Shared.Data.interceptores` | Sets `CreatedOn`/`CreatedById`/`LatestUpdatedOn`/`LatestUpdatedById` on `IAuditableEntity`s |
| `DispachDomainEventInterceptor` (`ISaveChangesInterceptor`) | `Shared.Data.interceptores` | Dispatches `IDomainEvent`s collected on entities (`IHasDomainEvents`) during `SaveChangesAsync` |

Both must be registered as scoped services in `Add{Module}` (see [modules.md](modules.md) → "Wiring a Module") and passed to `DbContextOptionsBuilder.AddInterceptors(...)`. Entities raise events via `EntityBase<TId>.AddDomainEvent(...)`; the interceptor publishes them on save.

## Entity Base Classes — `Shared.DDD`

```csharp
public abstract class EntityBase<TId> : IEntityBase<TId>, IHasDomainEvents
{
    public TId Id { get; protected set; } = default!;
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    protected void AddDomainEvent(IDomainEvent @event);
    public void ClearDomainEvents();
}

public interface IAuditableEntity            // from Shared.DDD
{
    DateTimeOffset CreatedOn { get; }
    string? CreatedById { get; }
    DateTimeOffset? LatestUpdatedOn { get; }
    string? LatestUpdatedById { get; }
}
```

- Use `IAuditableEntity` (not a fictional `IAuditable`) for auditable aggregates.
- There is **no `ISoftDelete`** interface. Model soft-delete as a plain per-entity property when needed (`public bool IsDeleted { get; private set; }`) and filter it in queries.
- Field names are `CreatedOn` / `CreatedById` / `LatestUpdatedOn` / `LatestUpdatedById` — **not** `CreatedAt`/`CreatedBy`/`ModifiedAt`/`ModifiedBy`.

## Entity Configuration — Fluent API + `{Impl}/Data/Configurations/`

Configurations are `internal sealed class : IEntityTypeConfiguration<T>` in `{Impl}/Data/Configurations/`, picked up by `ApplyConfigurationsFromAssembly`. **Always use the Fluent API, not data annotations.**

```csharp
namespace IdentityModule.Data.Configurations;

internal sealed class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasMany(x => x.Users).WithOne()
            .HasForeignKey(x => x.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Value objects backed by a single primitive string → HasConversion
        builder.Property(x => x.Name).HasConversion(v => v.name, v => new Name(v));
        builder.Property(x => x.Email).HasConversion(v => v.email, v => new Email(v));
        builder.Property(x => x.Description)
            .HasConversion(v => v.description, v => v == null ? null : new Description(v));
    }
}
```

### Value object mapping — `HasConversion` vs `OwnsOne`

Both patterns exist in the repo; pick by the value object's shape:

| Value object shape | Pattern | Example |
|---|---|---|
| Single primitive (e.g. `Name`, `Email`, `Description` in `Shared.DDD`) | `Property(...).HasConversion(v => v.X, v => new X(v))` | Identity `VendorConfiguration` |
| Multiple fields / nested structure (e.g. a `Money(Amount, Currency)`) | `builder.OwnsOne(p => p.Price, price => { price.Property(m => m.Amount)...; })` | used elsewhere in the repo when a VO has >1 field |

## Repository Pattern — `IGenericeRepository<T, DB>`

The generic repository is the real, shared abstraction in `Shared.Abstraction` (namespace `Shared.Abstraction`). Note the documented misspelling — the type is `IGenericeRepository` / `GenericeRepository` (sic, throughout the codebase).

### Interface (full surface)

```csharp
namespace Shared.Abstraction;

public interface IGenericeRepository<T, DB> where T : class where DB : DbContext
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync<TId>(TId id, bool trackChanges = false, params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync<TId>(TId id);
    Task<T> GetByIdAsync<TId>(TId id, CancellationToken ct);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<IQueryable<T>> GetAllAsQuerable();
    Task<IQueryable<T>> Query();            // compose LINQ reads on top of this
    void Add(T entity);
    void Update(T entity);
    void Delete<TId>(TId id);
    void DeleteRange<TId>(IEnumerable<TId> ids);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken ct);
    IDbContextTransaction BeginTransaction();
    Task UpdateAsync(T entity, CancellationToken ct);
}
```

**There is no `Specification<T>` type.** Reusable/filters are just LINQ built atop `Query()` / `GetAllAsQuerable()` (see Read patterns below).

### Registration — open generic, once per module

```csharp
services.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));
```

### Usage in handlers

```csharp
public class CreatVendorCommandHandler(IGenericeRepository<Vendor, IdentityDbContext> repo)
    : ICommandHandler<CreatVendorCommand>
{
    public async Task<Result> Handle(CreatVendorCommand request, CancellationToken ct)
    {
        var vendor = Vendor.Create(request.VendorDto.Name, request.VendorDto.Description,
                                    request.VendorDto.Email, request.VendorDto.IsActive);
        await repo.AddAsync(vendor, ct);
        await repo.SaveChangesAsync(ct);          // explicit save — Add/AddAsync do NOT commit
        return Result.Success();
    }
}
```

Update/delete equivalent (entity loaded, mutated, saved):

```csharp
var vendor = await repo.GetByIdAsync(request.Id);
if (vendor is null) return Result.Failure(Error.NullValue);

vendor.Update(request.VendorDto.Name, request.VendorDto.Description,
              request.VendorDto.Email, request.VendorDto.IsActive, safeSlug);
repo.Update(vendor);
await repo.SaveChangesAsync(ct);
```

**Always call `SaveChangesAsync` after writes** — the repository's `Add`/`Update`/`Delete` are tracked-only and do not persist by themselves.

## Read Patterns (no Specification — compose LINQ)

Project to DTOs at the database, not in memory:

```csharp
public class GetVendorByIdQueryHandler(IGenericeRepository<Vendor, IdentityDbContext> repo)
    : IQueryHandler<GetVendorByIdQuery, VendorDto>
{
    public async Task<Result<VendorDto>> Handle(GetVendorByIdQuery request, CancellationToken ct)
    {
        var vendor = await repo.GetByIdAsync(request.Id);
        if (vendor == null) return Result.Failure<VendorDto>(Error.NullValue);

        return Result.Success(new VendorDto(
            vendor.Id, vendor.Name.name, vendor.Slug, vendor.Email.email,
            vendor.Description.description, vendor.IsActive));
    }
}
```

For filtered/lists/paged reads build LINQ off `Query()` or `GetAllAsQuerable()` and `Select` to a DTO. Filters with includes compose via `GetAllAsync(include1, include2)`. Grid/paged lists in this repo use a `SmartTableParam` payload (see [api-conventions.md](api-conventions.md)).

> Prefer `AsNoTracking` for read-only paths by composing the `IQueryable` from `Query()` with `.AsNoTracking()` before materializing.

## Transactions — explicit (`BeginTransaction`)

There is **no implicit per-command transaction** (commands do not auto-transaction; the old "implicit transactions" guidance was fictional). Wrap multi-statement writes manually:

```csharp
public async Task<Result<Guid>> Handle(DeleteVendorCommand request, CancellationToken ct)
{
    var vendor = await repo.GetByIdAsync(request.Id);
    if (vendor is null) return Result.Failure<Guid>(Error.NullValue);

    using var transaction = repo.BeginTransaction();
    try
    {
        repo.Delete(vendor);
        await entityService.Remove(request.Id, EntityType);   // cross-module, same tx
        await transaction.CommitAsync(ct);
        await repo.SaveChangesAsync(ct);
        return Result.Success(request.Id);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync(ct);
        return Result.Failure<Guid>(Error.Problem("400", ex.Message));
    }
}
```

`BeginTransaction()` returns an `IDbContextTransaction`; call `CommitAsync` / `RollbackAsync` with the `CancellationToken`.

## Migrations — in-module, auto-applied

Migrations live at `{Impl}/Data/Migrations/` (e.g. `IdentityModule/Data/Migrations/`, `Catalog/Data/Migrations/`). There is **no Playground `Migrations.PostgreSQL` project**.

```bash
# Add a migration for module {X}DbContext — run from the solution root,
# targeting the module's implementation project where the DbContext lives
dotnet ef migrations add MyChange `
    --project Src/Modules/{Module}/{Impl} `
    --context {X}DbContext `
    --output-dir Data/Migrations

# Apply manually (optional — usually applied automatically on startup)
dotnet ef database update `
    --project Src/Modules/{Module}/{Impl} `
    --context {X}DbContext
```

### Auto-apply on startup

`Program.cs` chains `app.Use{Module}()` which calls `app.UseMigration<{X}DbContext>()` (`Shared.Data.Extention`). That helper creates a service scope, runs `context.Database.MigrateAsync()`, then invokes `IDbInitializer.SeedAsync()` — so migrations + seed run on boot.

```csharp
// Shared.Data.Extention
public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
    where TContext : DbContext
{
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    var seeder   = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await seeder.SeedAsync();
    await context.Database.MigrateAsync();
    return app;
}
```

Each module provides an `IDbInitializer` implementation (namespace `Shared.Persistence`) registered as scoped: `services.AddScoped<IDbInitializer, {Module}DbInitializer>();`.

## Outbox / Integration events

Each module's DbContext carries a `DbSet<OutboxMessage> OutboxMessages` (`Shared.Eventing`). `services.AddEventingForDbContext<{X}DbContext>()` wires outbox persistence + dispatch; handlers persist `OutboxMessage` rows inside the same write, and `AddIntegrationEventHandlers(typeof({X}Module).Assembly)` registers subscribers. Inter-module integration events go through Contract events (see [modules.md](modules.md)).

## Common Pitfalls

### ❌ Forgetting to save

```csharp
await repo.AddAsync(vendor, ct);     // tracked, NOT persisted
// repo.SaveChangesAsync(ct);        // ❌ missing → nothing is saved
```
Always follow add/update/delete with `SaveChangesAsync(ct)`.

### ❌ Loading full entities for reads

```csharp
// ❌ load full graph, map in memory
var products = await repo.GetAllAsync();
return products.Select(p => new ProductDto(...)).ToList();
```
```csharp
// ✅ project to DTO in the database
var q = await repo.Query();
return await q.Where(p => !p.IsDeleted)
             .Select(p => new ProductDto(p.Id, p.Name))
             .AsNoTracking()
             .ToListAsync(ct);
```

### ❌ Lazy loading

Lazy loading is **not configured**. Accessing a navigation you didn't include (or `Query()`-compose) returns navigation, returns `null`/empty. Use eager includes (`GetAllAsync(include)`, `.Include(...)`) or explicit composition.

### ❌ Detaching / manual state manipulation

```csharp
dbContext.Entry(product).State = EntityState.Detached;   // ❌
```
Go through `IGenericeRepository<,>` (`Add`/`Update`/`Delete` + `SaveChangesAsync`).

### ❌ N+1

```csharp
var vendors = await repo.GetAllAsync();
foreach (var v in vendors) { var s = await repo.GetByIdAsync(v.Id); }   // ❌ N queries
```
Include child collections up front (`GetAllAsync(include)`) or build a single projection.

## Key Rules

1. **One DbContext per module**, each with its own `HasDefaultSchema(...)`.
2. **Plain `DbContext`** (or `IdentityDbContext<,,>`) — no shared base; audit + events via **interceptors**.
3. **Entities derive from `EntityBase<TId>`**; auditable ones add `IAuditableEntity` (`CreatedOn`/`CreatedById`/`LatestUpdatedOn`/`LatestUpdatedById`).
4. **Fluent API only**; value objects via `HasConversion` (single primitive) or `OwnsOne` (multi-field).
5. **`IGenericeRepository<,>`** registered open-generic per module — no `Specification<T>`; compose reads via `Query()`/`GetAllAsQuerable()`.
6. **Call `SaveChangesAsync` after every write**; transactions are explicit (`BeginTransaction`/`Commit`/`Rollback`).
7. **Migrations in-module** (`{Impl}/Data/Migrations/`), auto-applied on startup (`UseMigration<TContext>()` + `IDbInitializer`).
8. **Project to DTOs in the database** for read paths; `AsNoTracking` for read-only.

---

For migration help: `migration-helper` agent. For module wiring / endpoint dispatch, see [modules.md](modules.md) and [api-conventions.md](api-conventions.md).