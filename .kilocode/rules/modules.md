---
paths:
  - "Src/Modules/**"
---

# Module Rules

Modules are **bounded contexts** in the modular monolith. Each module is self-contained and owns its own DbContext, domain, and feature slices.

## Repository Reality (read this first)

The names of module **projects** and **folders are not uniform across the repo** — different modules were authored at different times. Extending an existing module? Match *its* conventions; do not rename or "fix" its projects to match another module. Creating a brand-new module? Use the canonical pattern below.

| Module | Implementation project | Contract project |
|--------|------------------------|-------------------|
| Identity | `IdentityModule` | `Module.Identity.Contract` |
| Catalog | `Catalog` | `CatalogContract` |
| Basket | `Eshop.Module.Basket` | `Eshop.Module.Basket.Contract` |
| Inventory | `Module.Inventory` | `Module.Inventory.Contract` |
| Core | `Eshop.Module.Core` | `EShop.Module.Core.Contract` |
| Localization | `Eshop.Module.Localization` | `Eshop.Module.Localization.Contract` |
| Pricing | `Eshop.Module.Pricing` | _(no contract yet)_ |

Namespaces follow the project name (e.g. `Module.Identity.Contract.*`, `IdentityModule.*`, `Catalog.Features.*`, `EShop.Module.Core.Contract.*`). Yes, the `Module.`/`Eshop.`/`Catalog` prefixes differ — preserve what exists.

## Canonical Structure

Derived from the most recent complete work (Identity `Vendors`). Use this for new modules:

```
Modules/{ModuleName}/
├── {Impl}.csproj                 # implementation (internal)
│   ├── {Impl}.cs                 # static Add{Module}() / Use{Module}()
│   ├── Feature/                  # CQRS vertical slices (singular "Feature")
│   │   └── {Entity}/{ActionFeature}/
│   │       ├── {Action}CommandHandler.cs      # or QueryHandler
│   │       └── {Action}EndPoint.cs             # ICarterModule
│   ├── Domain/  (or Models/)     # domain entities + value objects
│   ├── Data/                     # DbContext, configurations, migrations
│   │   ├── {Module}DbContext.cs
│   │   ├── Configurations/       # IEntityTypeConfiguration<T>
│   │   └── Migrations/           # EF Core migrations for THIS module
│   ├── Services/                 # module-local application services
│   │   └── {Module}DbInitializer.cs  # implements IDbInitializer (seed)
│   └── Events/                   # integration event handlers
└── {Contract}.csproj             # public interface
    ├── Dtos/                     # public record DTOs
    ├── Feature/{Entity}/         # public Command + Query records
    ├── Services/                 # I{Module}Service interfaces
    └── Events/                   # integration event contracts
```

### Important deviations from the structure above

- **Commands and Queries** may live in the **Contract** project (Identity: `Module.Identity.Contract/Feature/Vendors/CreatVendorCommand.cs`) **or** co-located with the handler (Catalog: `Catalog/Features/Brands/AddBrand/CreatBrandCommand.cs`). When extending Identity-style modules, put them in Contract; when extending Catalog-style modules, keep them in the implementation. **DTOs always live in Contract.**
- **Feature vs Features**: Identity/Core use `Feature/` (singular); Catalog/Inventory use `Features/`. Match the module.
- **Domain vs Models**: Identity uses `Domain/`; Basket/Core/Inventory/Localization use `Models/`. Match the module.
- There is **no `v1` version folder** anywhere. Do not introduce one unless asked.
- Each **feature folder is its own `ICarterModule`** (the `*EndPoint.cs` file). There is no central `{Module}Endpoints` aggregator and no `group.MapCreateXEndpoint()` wiring — Carter discovers every `ICarterModule` in the assembly automatically (see Program.cs below).

## Module Independence

### ✅ Allowed

```csharp
// Reference another module's Contract project (e.g. Core.Contract)
using EShop.Module.Core.Contract.Services;
// injected cross-module service
public class CreatVendorCommandHandler(IEntityService entityService, ...) { }
```

```csharp
// Use BuildingBlocks
using Shared.Contract.CQRS;     // ICommand / IQuery / handlers
using Shared.Contract.ResultPattern; // Result / Error
using Shared.Abstraction;       // IGenericeRepository<,>
using Shared.DDD;               // EntityBase<TId>, value objects
```

### ❌ Forbidden

```csharp
using IdentityModule;            // ❌ Never reference another module's internals
using IdentityModule.Domain;     // ❌ Domain entities are internal to the module
```

## Cross-Module Communication

### Option 1: Contract service interface (preferred)

The real, working example — Identity's Vendor handlers depend on Core's entity-slug service via its Contract:

```csharp
// EShop.Module.Core.Contract/Services/IEntityService.cs
public interface IEntityService
{
    Task<string> ToSafeSlug(string slug, Guid entityId, string entityType);
    Task Add(string name, string slug, Guid entityId, string entityType);
    Task Update(string name, string slug, Guid entityId, string entityType);
    Task Remove(Guid entityId, string entityType);
}

// IdentityModule implementation consumes it:
public class CreatVendorCommandHandler(
    IGenericeRepository<Vendor, IdentityDbContext> vendorRepository,
    IEntityService entityService)   // ← cross-module contract dependency
    : ICommandHandler<CreatVendorCommand> { ... }
```

### Option 2: Integration events (outbox)

```csharp
// Raising: handler persists an OutboxMessage (DbSet<OutboxMessage> on the DbContext)
// Consuming: class in {Impl}/Events/ implements the integration event handler
//            and is registered via services.AddIntegrationEventHandlers(typeof(XModule).Assembly);
```

`AddEventingForDbContext<XDbContext>()` wires the outbox + dispatch interceptors for a module's DbContext.

## CQRS (MediatR) — the real interfaces

From `Shared.Contract.CQRS`:

```csharp
public interface ICommand  : IRequest<Result>;           // returns Result
public interface ICommand<T> : IRequest<Result<T>>;      // returns Result<T>
public interface IQuery<T>  : IRequest<Result<T>>;       // returns Result<T>

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>;
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>;
```

- Handlers return `Task<Result>` / `Task<Result<T>>` — **never `ValueTask`**.
- Success/failure is expressed with `Result`, not exceptions or raw values.

```csharp
// Command (in Contract)
public record CreatVendorCommand(CreatVendorDto VendorDto) : ICommand;

// Handler (in implementation)
public class CreatVendorCommandHandler(IGenericeRepository<Vendor, IdentityDbContext> repo)
    : ICommandHandler<CreatVendorCommand>
{
    public async Task<Result> Handle(CreatVendorCommand request, CancellationToken ct)
    {
        var vendor = Vendor.Create(request.VendorDto.Name, request.VendorDto.Description,
                                    request.VendorDto.Email, request.VendorDto.IsActive);
        await repo.AddAsync(vendor, ct);
        await repo.SaveChangesAsync(ct);
        return Result.Success();
    }
}
```

## Domain Entities (Shared.DDD)

Entities derive from `EntityBase<TId>` (not a fictional `BaseEntity`) and optionally implement `IAuditableEntity`. There is **no `BaseEntity` and no `ISoftDelete` interface** in BuildingBlocks; soft-delete is a plain per-entity property (`bool IsDeleted`) when needed.

```csharp
public class Vendor : EntityBase<Guid>, IAuditableEntity
{
    public Name Name { get; private set; }            // value objects from Shared.DDD
    public string Slug { get; private set; }
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // IAuditableEntity: CreatedOn / CreatedById / LatestUpdatedOn / LatestUpdatedById
    // (NOT CreatedAt/CreatedBy/ModifiedAt/ModifiedBy)

    public static Vendor Create(string name, string description, string email, bool isActive)
        => new() { Id = Guid.NewGuid(), Name = new Name(name),
                   Email = new Email(email), IsActive = isActive,
                   CreatedOn = DateTimeOffset.UtcNow };

    public void Update(string name, string email, bool isActive, string slug) { /* set fields */ }
}
```

- `EntityBase<TId>` provides `Id`, `DomainEvents`, `AddDomainEvent(...)`, `ClearDomainEvents()`.
- Value objects (`Name`, `Email`, `Description`, …) live in `Shared.DDD` and are mapped via EF value converters (see [persistence.md](persistence.md)).

## Wiring a Module (DI + hosting)

Each module exposes a **static `{Module}Module` class** with `Add{Module}` and `Use{Module}` extension methods. `Program.cs` chains them:

```csharp
// Program.cs (real)
builder.Services.AddCatalog(builder.Configuration)
    .AddCore(builder.Configuration)
    .AddBasket(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddInventory(builder.Configuration);

app.UseCatalog().UseCore().UseBasket().UseIdentity().UseInventory();

// Carter auto-discovers every ICarterModule in each module assembly
builder.Services.AddCarter(configurator: config =>
{
    var identityModuleTypes = typeof(IdentityModule).Assembly
        .GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
    config.WithModules(identityModuleTypes);
    // ...repeat per module...
});

// MediatR registers module assemblies
builder.Services.AddMediatRWithAssemblies(catalogAssembly, basketAssembly, identityAssembly);
```

A typical `Add{Module}`:

```csharp
public static class CatalogModule
{
    public static IServiceCollection AddCatalog(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(CatalogModule).Assembly));

        // domain-event dispatch + audit via interceptors (Shared.Data)
        services.AddScoped<ISaveChangesInterceptor, DispachDomainEventInterceptor>();
        services.AddScoped<DispachDomainEventInterceptor>();
        services.AddScoped<AuditableEntityInterceptor>();

        // open-generic repository for entities in this module
        services.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));

        services.AddEventingForDbContext<CatalogDbContext>();
        services.AddIntegrationEventHandlers(typeof(CatalogDbContext).Assembly);

        services.AddDbContext<CatalogDbContext>((sp, opt) =>
        {
            opt.AddInterceptors(sp.GetRequiredService<DispachDomainEventInterceptor>())
               .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>())
               .UseNpgsql(cfg.GetConnectionString("defualt")); // key is "defualt" in this repo
        });

        services.AddScoped<IDbInitializer, CatalogDbInitializer>();
        return services;
    }

    public static IApplicationBuilder UseCatalog(this IApplicationBuilder app)
        => app.UseMigration<CatalogDbContext>();   // MigrateAsync() + SeedAsync() on startup
}
```

## Creating a New Module

1. **Create the two projects** (canonical naming: `Module.{Name}` impl + `Module.{Name}.Contract`). Pick names and stick with them internally.

   ```bash
   dotnet new classlib -n Module.{Name} -o Src/Modules/{Name}/Module.{Name}
   dotnet new classlib -n Module.{Name}.Contract -o Src/Modules/{Name}/Module.{Name}.Contract
   ```

2. **Add to solution** and register in `EshopModulith.slnx`.

3. **Reference BuildingBlocks + own Contract** in the implementation `.csproj`:

   ```xml
   <ItemGroup>
     <PackageReference Include="Microsoft.EntityFrameworkCore" />
     <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" />
     <ProjectReference Include="..\{Contract}\Module.{Name}.Contract.csproj" />
     <ProjectReference Include="..\..\..\BuildingBlocks\Shared\Shared.csproj" />
     <ProjectReference Include="..\..\..\BuildingBlocks\Shared.Contract\Shared.Contract.csproj" />
     <ProjectReference Include="..\..\..\BuildingBlocks\Shared.Eventing\Shared.Eventing.csproj" />
   </ItemGroup>
   ```

4. **Scaffold**: `Domain`/`Models` entities → `Data` DbContext + `Configurations` + `Migrations` → `Feature` slices → `Services` + `DbInitializer` → static `{Name}Module` (`Add{Module}`/`Use{Module}`).
5. **Wire into `Program.cs`**: add `Add{Module}` to the service chain, `Use{Module}` to the app chain, register the assembly with Carter's `WithModules(...)`, and add the assembly to `AddMediatRWithAssemblies(...)`.

## Key Rules

1. **Contracts are public**; module internals are `internal`.
2. **Modules communicate via Contracts or integration events** — never direct project references to another module's implementation.
3. **Each module has its own DbContext** (plain `DbContext`, or `IdentityDbContext<,,>` for Identity) with its own schema.
4. **Features are vertical slices**; each slice's endpoint is its own auto-discovered `ICarterModule`.
5. **BuildingBlocks are shared** — don't modify them without approval (see [buildingblocks-protection.md](buildingblocks-protection.md)).

---

For scaffolding help: `/add-module` skill or `module-creator` agent. For entities/persistence specifics see [persistence.md](persistence.md); for endpoints see [api-conventions.md](api-conventions.md).