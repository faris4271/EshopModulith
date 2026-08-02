---
paths:
  - "src/**"
---

# Architecture Rules

EshopModulith is a **Modular Monolith** — NOT microservices, NOT a traditional layered architecture.

## Core Principles

### 1. Modular Monolith

```
Single deployment unit
    ↓
Multiple bounded contexts (modules)
    ↓
Each module is self-contained
    ↓
Communication via Contracts (interfaces/DTOs)
```

**Modules:**
- Catalog (Product catalog)
- Basket (Shopping cart)
- Inventory (Stock management)
- Identity (Auth, users, roles, permissions)
- Core (Core domain entities)
- Localization (Multi-language support)
- Pricing (Price management)

**Rules:**
- Modules CANNOT reference other module internals
- Modules CAN reference other module Contracts
- Modules share BuildingBlocks (framework code)

### 2. CQRS (MediatR Library)

**Commands** (write operations):
```csharp
public record CreateUserCommand(string Email) : ICommand<Guid>;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        // Write to database
        return user.Id;
    }
}
```

**Queries** (read operations):
```csharp
public record GetUserQuery(Guid Id) : IQuery<UserDto>;

public class GetUserHandler : IQueryHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery query, CancellationToken ct)
    {
        // Read from database
        return userDto;
    }
}
```

FSH uses the **MediatR** library with custom `ICommand`/`IQuery` interfaces from BuildingBlocks.

### 3. Domain-Driven Design

**Entities** inherit `BaseEntity`:
```csharp
public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public Money Price { get; private set; } = default!;
    
    public static Product Create(string name, Money price)
    {
        // Factory method, enforce invariants
        return new Product { Name = name, Price = price };
    }
}
```

**Value Objects** (immutable):
```csharp
public record Money(decimal Amount, string Currency);
```

**Aggregates:**
- Root entity controls access to child entities
- Enforce business rules
- Transaction boundary

### 4. Vertical Slice Architecture

Each feature = complete slice (command/handler/validator/endpoint in one folder).

```
Features/v1/CreateProduct/
├── CreateProductCommand.cs
├── CreateProductHandler.cs
├── CreateProductValidator.cs
└── ProductEndpoints.cs (ICarterModule)
```

**Benefits:**
- High cohesion (related code together)
- Low coupling (features don't depend on each other)
- Easy to find/modify

### 5. BuildingBlocks (Shared Kernel)

10 packages providing cross-cutting concerns:

| Package | Purpose |
|---------|---------|
| Shared | Core DTOs, constants, utilities |
| Shared.Caching | Redis/memory caching |
| Shared.Contract | CQRS interfaces (ICommand, IQuery), shared contracts |
| Shared.Eventing | Domain events, message bus implementation |
| Shared.Eventing.Contract | Event contracts |
| Shared.Jobs | Background jobs |
| Shared.Mailing | Email templates, mailing integration |
| Shared.Message | Message patterns |
| Shared.Storage | File storage (local, cloud) |
| Shared.Web | API conventions, filters, middleware |

**Protected:** BuildingBlocks should NOT be modified without approval. See `.claude/rules/buildingblocks-protection.md`.

### 6. Dependency Flow

```
API Layer (Carter Modular Routing)
    ↓
Application Layer (Commands/Queries/Handlers)
    ↓
Domain Layer (Entities/Value Objects)
    ↓
Infrastructure Layer (Persistence/External Services)
```

**Rules:**
- Domain CANNOT depend on infrastructure
- Application CANNOT depend on infrastructure directly
- Infrastructure implements domain interfaces

### 7. Persistence Strategy

**DbContext per Module:**
- Each module has its own DbContext (e.g., CatalogDbContext, IdentityDbContext).

**Repository Pattern:**
```csharp
public interface IGenericeRepository<T, TDbContext> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<T>> ListAsync(Specification<T> spec, CancellationToken ct);
    Task<T> AddAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task DeleteAsync(T entity, CancellationToken ct);
}
```

**Specification Pattern** (queries):
```csharp
public class ActiveProductsSpec : Specification<Product>
{
    public ActiveProductsSpec()
    {
        Query.Where(p => !p.IsDeleted && p.IsActive);
    }
}
```

## Architectural Tests

`Architecture.Tests` project (planned/future) will enforce rules:

```csharp
[Fact]
public void Modules_Should_Not_Reference_Other_Modules()
{
    // Fails if Module A references Module B directly
}

[Fact]
public void Domain_Should_Not_Depend_On_Infrastructure()
{
    // Fails if domain entities reference EF Core
}
```

## Technology Stack

- **.NET 10** (latest LTS)
- **EF Core 10** (PostgreSQL provider)
- **MediatR** (CQRS)
- **Carter** (Modular Routing)
- **FluentValidation** (input validation)
- **Mapster** (object mapping)
- **Hangfire** (background jobs)
- **Swagger** (OpenAPI docs)
- **MailKit** (email)
- **Serilog** (logging)
- **OpenTelemetry** (observability)
- **Aspire** (orchestration)

## Key Takeaways

1. **Modular Monolith** ≠ Microservices. Modules share process, database, infrastructure.
2. **CQRS** separates reads/writes. Use `ICommand`/`IQuery` via MediatR.
3. **DDD** enforces business rules in domain. Entities control their state.
4. **Carter** handles modular endpoint registration.
5. **Vertical Slices** keep features independent. No shared "services" layer.
6. **BuildingBlocks** provide infrastructure. Don't reinvent, reuse.
7. **Tests enforce architecture**. Violate rules → build fails.

---

For implementation details, see:
- `ARCHITECTURE_ANALYSIS.md` (deep dive)
- `.claude/rules/modules.md` (module patterns)
- `.claude/rules/persistence.md` (data access patterns)
