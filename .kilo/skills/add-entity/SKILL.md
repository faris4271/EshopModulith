---
name: add-entity
description: Create a domain entity with auditing, soft-delete, and domain events. Use when adding new database entities to a module.
argument-hint: [ModuleName] [EntityName]
---

# Add Entity

Create a domain entity following EshopModulith patterns.

## Entity Template

```csharp
using Shared.DDD;

namespace Eshop.Module.{Module}.{Entity}s.Models;

public sealed class {Entity} : EntityBase, IAuditableEntity, ISoftDelete
{
    // Domain properties
    // Add your entity properties here

    // Private constructor for EF Core
    private {Entity}() { }

    // Factory method - the only way to create
    public static {Entity} Create(/* Add parameters */)
    {
        // Validation
        // Create and return entity
        
        entity.AddDomainEvent({Entity}CreatedEvent.Create(entity));
        return entity;
    }

    // Domain methods for state changes
    public void Update(/* Add parameters */)
    {
        // Validation
        // Update properties
        
        AddDomainEvent({Entity}UpdatedEvent.Create(this, changedProperties));
    }

    public void Delete()
    {
        IsDeleted = true;
        AddDomainEvent({Entity}DeletedEvent.Create(Id));
    }
}
```

```csharp
using Shared.DDD;

namespace {Module}.{Entity}s.Models;

public sealed class {Entity} : EntityBase, IAuditableEntity, ISoftDelete
{
    // Domain properties
    public Name Name { get; private set; } = default!;
    public Money Price { get; private set; } = default!;
    public Description? Description { get; private set; }

    // Private constructor for EF Core
    private {Entity}() { }

    // Factory method - the only way to create
    public static {Entity} Create(Name name, Money price, Description? description)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(price);

        var entity = new {Entity}
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price,
            Description = description
        };

        entity.AddDomainEvent({Entity}CreatedEvent.Create(entity));
        return entity;
    }

    // Domain methods for state changes
    public void UpdateDetails(Name name, Money price, Description? description)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(price);

        Name = name;
        Price = price;
        Description = description;

        AddDomainEvent({Entity}UpdatedEvent.Create(this, new[] { nameof(Name), nameof(Price), nameof(Description) }));
    }

    public void Delete()
    {
        IsDeleted = true;
        AddDomainEvent({Entity}DeletedEvent.Create(Id));
    }
}
```

## Domain Events

### Option 1: Entity Reference Events (Lightweight)

Use when you only need the entity ID for downstream processing.

```csharp
public sealed record {Entity}CreatedEvent(Guid EventId, Guid {Entity}Id, DateTimeOffset OccurredOnUtc)
    : DomainEvent(EventId, OccurredOnUtc)
{
    public {Entity}CreatedEvent(Guid eventId, Guid {Entity}Id)
        : this(eventId, {Entity}Id, DateTimeOffset.UtcNow) { }
}

public sealed record {Entity}UpdatedEvent(Guid EventId, Guid {Entity}Id, DateTimeOffset OccurredOnUtc)
    : DomainEvent(EventId, OccurredOnUtc)
{
    public {Entity}UpdatedEvent(Guid eventId, Guid {Entity}Id)
        : this(eventId, {Entity}Id, DateTimeOffset.UtcNow) { }
}

public sealed record {Entity}DeletedEvent(Guid EventId, Guid {Entity}Id, DateTimeOffset OccurredOnUtc)
    : DomainEvent(EventId, OccurredOnUtc)
{
    public {Entity}DeletedEvent(Guid eventId, Guid {Entity}Id)
        : this(eventId, {Entity}Id, DateTimeOffset.UtcNow) { }
}
```

### Option 2: Full Entity Events (Eager Payload) — **Recommended Pattern**

Use when consumers need the complete entity data immediately (e.g., for projections, notifications, or sync). This avoids additional queries.

```csharp
using {Module}.{Entity}s.Models;
using Shared.DDD;

namespace {Module}.{Entity}s.Events;

public sealed record {Entity}CreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    {Entity} {Entity} // Full entity payload
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static {Entity}CreatedEvent Create({Entity} {entity})
    {
        return new {Entity}CreatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            {entity}
        );
    }
}

public sealed record {Entity}UpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    {Entity} {Entity},
    string[] ChangedProperties // Track what changed
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static {Entity}UpdatedEvent Create({Entity} {entity}, string[] changedProperties)
    {
        return new {Entity}UpdatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            {entity},
            changedProperties
        );
    }
}

public sealed record {Entity}DeletedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    Guid {Entity}Id // Only ID needed for deletion
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static {Entity}DeletedEvent Create(Guid {entity}Id)
    {
        return new {Entity}DeletedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            {entity}Id
        );
    }
}
```

**Key differences:**
| Aspect | Option 1 (Reference) | Option 2 (Full Entity) |
|--------|---------------------|------------------------|
| Payload size | Small (GUID only) | Larger (full entity) |
| Consumer coupling | Loose (fetch if needed) | Tight (has full data) |
| Use case | Audit, async processing | Real-time sync, projections, notifications |
| Serialization | Trivial | Requires entity to be serializable |

## EF Core Configuration

```csharp
using Shared.DDD;

internal sealed class {Entity}Configuration : IEntityTypeConfiguration<{Entity}>
{
    public void Configure(EntityTypeBuilder<{Entity}> builder)
    {
        builder.ToTable("{entities}", "{module}");

        builder.HasKey(x => x.Id);

        builder.Property(c => c.Name)
            .HasConversion(a => a.name, name => new(name));

        builder.Property(d => d.Description)
            .HasConversion(d => d.description, v => new Description(v));

        builder.OwnsOne(x => x.Price, m =>
        {
            m.Property(p => p.Amount)
                .HasColumnName("PriceAmount")
                .HasPrecision(18, 4)
                .IsRequired();

            m.Property(p => p.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Global query filter for soft delete
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}
```

## Register in DbContext

```csharp
public sealed class {Module}DbContext : BaseDbContext
{
    public DbSet<{Entity}> {Entities} => Set<{Entity}>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("{module}");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof({Module}DbContext).Assembly);
    }
}
```

## Add Migration

```bash
dotnet ef migrations add Add{Entity} \
  --project Src/Playground/Migrations.PostgreSQL \
  --startup-project Src/Api/Api \
  --context {Module}DbContext

dotnet ef database update \
  --project Src/Playground/Migrations.PostgreSQL \
  --startup-project Src/Api/Api \
  --context {Module}DbContext
```

**Variants:**
```csharp
// Updated event with changed properties
public sealed record {Entity}UpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    {Entity} {Entity},
    string[] ChangedProperties
) : DomainEvent(EventId, OccurredOnUtc);

// Deleted event with entity ID only (entity is soft-deleted)
public sealed record {Entity}DeletedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    Guid EntityId
) : DomainEvent(EventId, OccurredOnUtc);
```

## Key Rules

1. **Private constructor** - EF Core needs it, but users use factory methods
2. **Factory methods** - All creation goes through `Create()` static method
3. **Domain methods** - State changes through methods, not property setters
4. **Domain events** - Raise events for significant state changes using factory methods:
   - `AddDomainEvent({Entity}CreatedEvent.Create(this));` in `Create()`
   - `AddDomainEvent({Entity}UpdatedEvent.Create(this, changedProperties));` on updates
   - `AddDomainEvent({Entity}DeletedEvent.Create(Id));` on delete
5. **Validation in methods** - Validate in factory/domain methods, not entity
6. **No public setters** - Properties are `private set`
7. **Entity namespace** - Events reference `using {Module}.{Entity}s.Models;` for full entity payload

## Checklist

- [ ] Inherits `EntityBase`
- [ ] Implements `IAuditableEntity` for audit trails
- [ ] Implements `ISoftDelete` for soft deletes
- [ ] Has private constructor
- [ ] Has static factory method
- [ ] Domain events raised for state changes using factory pattern (`{Entity}CreatedEvent.Create(this)`)
- [ ] Events in separate `{Module}.{Entity}s.Events` namespace
- [ ] Events reference `using {Module}.{Entity}s.Models;`
- [ ] EF configuration created
- [ ] Added to DbContext
- [ ] Migration created

(End of file - total 164 lines)
