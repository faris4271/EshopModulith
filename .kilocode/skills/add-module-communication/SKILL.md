---
name: add-module-communication
description: Create synchronous communication between modules using MediatR. Use when Module A needs to call a Query or Command in Module B (via Module B's Contract project).
---

# Add Module Communication (Sync via MediatR)

Create synchronous cross-module communication using MediatR. Module A sends a Query/Command to Module B via Module B's **Contract** project.

## Architecture Pattern

```
Module A (Impl) ────── references ──────▶ Module B (Contract)
     │                                          │
     │         ISender.Send(Query/Command)     │
     ▼                                          ▼
ISender ─────────────────▶ Module B (Impl) handles it
```

**Key Rule:** Module A **must only reference** Module B's **Contract project** (not impl). The Contract exposes the Query/Command records and DTOs. Module B's implementation handles the request.


## Step 1: Define the Query/Command in Module B's Contract

Create the query/command record in Module B's Contract project:

```csharp
// Src/Modules/{ModuleB}/{ModuleB}.Contract/Features/{Entity}/{Action}/{Action}{Entity}Query.cs
using ModuleB.Contract.Dtos;
using Shared.Contract.CQRS;

namespace ModuleB.Contract.Features.{Entity}.{Action}
{
    public record Get{Entity}ByIdsQuery(List<Guid> Ids) : IQuery<List<{Entity}Dto>>;
}
```

**Key points:**
- Must be `public record` (not `internal`)
- Inherits from `IQuery<TResponse>` or `ICommand<TResponse>` from `Shared.Contract.CQRS`
- Uses DTOs from the same Contract's `Dtos` folder


## Step 2: Implement Handler in Module B's Implementation

```csharp
// Src/Modules/{ModuleB}/{ModuleB}/Features/{Entity}/{Action}/Get{Entity}ByIdsQueryHandler.cs
using ModuleB.Contract.Dtos;
using ModuleB.Contract.Features.{Entity}.{Action};
using ModuleB.Data;
using ModuleB.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace ModuleB.Features.{Entity}.{Action}
{
    internal class Get{Entity}ByIdsQueryHandler(
        IGenericeRepository<{Entity}, {ModuleB}DbContext> repository)
        : IQueryHandler<Get{Entity}ByIdsQuery, List<{Entity}Dto>>
    {
        public async Task<Result<List<{Entity}Dto>>> Handle(
            Get{Entity}ByIdsQuery query, 
            CancellationToken ct)
        {
            var entities = await repository.GetAllAsQuerable()
                .Where(e => query.Ids.Contains(e.Id))
                .Select(e => new {Entity}Dto(e.Id, e.Name, e.Price))
                .ToListAsync(ct);

            return Result.Success(entities);
        }
    }
}
```

**Key points:**
- Internal class (implementation detail)
- Uses `IQueryHandler<TQuery, TResponse>` from `Shared.Contract.CQRS`
- Returns `Task<Result<T>>` — never `ValueTask`
- Maps domain entities to Contract DTOs


## Step 3: Call from Module A (Consumer)

Module A references Module B's **Contract** project:

```xml
<!-- Module A's .csproj -->
<ProjectReference Include="..\{ModuleB}\{ModuleB}.Contract\{ModuleB}.Contract.csproj" />
```

Then in Module A's handler or service:

```csharp
// In Module A's handler or service
using ModuleB.Contract.Features.{Entity}.{Action};
using ModuleB.Contract.Dtos;
using MediatR;
using Shared.Contract.CQRS;

public class SomeCommandHandler(
    ISender sender) : ICommandHandler<SomeCommand>
{
    public async Task<Result> Handle(SomeCommand cmd, CancellationToken ct)
    {
        // Send query to Module B
        var query = new Get{Entity}ByIdsQuery(cmd.EntityIds);
        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        var entities = result.Value;
        // Use entities...

        return Result.Success();
    }
}
```


## Step 4: Wire Endpoint (Optional)

If Module A exposes an endpoint that triggers the cross-module call:

```csharp
// Module A Endpoint
using Carter;
using MediatR;
using ModuleA.Contract.Features.SomeFeature;
using Shared.Contract.ResultPattern;

namespace ModuleA.Features.SomeFeature;

internal sealed class SomeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/module-a/some-action", async (
            SomeCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Match(
                _ => Results.NoContent(),
                err => Results.BadRequest(err.Message));
        })
        .WithName(nameof(SomeCommand))
        .WithSummary("Do something that calls Module B")
        .AllowAnonymous();
    }
}
```


## Real Examples in Codebase

### Basket → Catalog (CartService calls Catalog)
```csharp
// Basket impl: Src/Modules/Basket/Eshop.Module.Basket/Services/CartService.cs
using CatalogContract.Services;  // Contract reference

var result = await _productService.GetProductsByIds(productIds);
// ProductService impl sends GetProductsByIdsQuery via ISender
```

### Catalog → Basket (Catalog calls Basket's Query)
```csharp
// Catalog impl: Src/Modules/Catalog/Catalog/Features/Products/Querys/GetProductsByIds/GetProductsByIdsQueryHandler.cs
using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Contract.Feature.GetProductsByIds;
using MediatR;

internal class GetProductsByIdsQueryHandler(
    ...
    ISender sender) : IQueryHandler<GetProductsByIdsQuery, List<CartItemDto>>
{
    public async Task<Result<List<CartItemDto>>> Handle(...)
    {
        var query = new GetProductsByIdsQuery(request.ProductIds);
        var result = await sender.Send(query);  // Calls Basket's handler
        ...
    }
}
```

### Identity → Core (Identity calls Core's EntityService)
```csharp
// Identity impl: IdentityModule/Feature/Vendors/CreatVendors/CreatVendorCommandHandler.cs
using EShop.Module.Core.Contract.Services;

internal class CreatVendorCommandHandler(
    IGenericeRepository<Vendor, IdentityDbContext> vendorRepository,
    IEntityService entityService)  // Core's contract interface
    : ICommandHandler<CreatVendorCommand>
{
    var safeSlug = await _entityService.ToSafeSlug(...);
```

## Checklist

- [ ] Query/Command record in **Module B Contract** project (public record)
- [ ] Inherits `IQuery<T>` or `ICommand<T>` from `Shared.Contract.CQRS`
- [ ] DTOs in **Module B Contract/Dtos** (public)
- [ ] Handler in **Module B Implementation** (internal class)
- [ ] Handler uses `IQueryHandler<T, R>` / `ICommandHandler<T, R>`
- [ ] Handler returns `Task<Result<T>>` (NOT `ValueTask`)
- [ ] Module A references Module B.**Contract** only (not Implementation)
- [ ] Module A uses `ISender` (MediatR) from `MediatR` namespace
- [ ] `await sender.Send(query/command, cancellationToken)`
- [ ] Handle `Result<T>` with `.IsSuccess` / `.Value` / `.Error`
- [ ] Build passes: `dotnet build EshopModulith.slnx` (0 warnings)


## Common Mistakes

| Mistake | Fix |
|---------|-----|
| Module A references Module B Implementation | Reference **Contract only** |
| Using `IRequest<T>` instead of `IQuery<T>`/`ICommand<T>` | Use `Shared.Contract.CQRS` interfaces |
| Handler returns `ValueTask` | Use `Task<T>` |
| Forgetting `CancellationToken` in handler | Include `CancellationToken ct` parameter |
| Not handling `Result<T>` failure | Check `result.IsSuccess` / use `result.Match()` |
| DTOs in Implementation instead of Contract | Move DTOs to Contract project |
| Using internal query/command in Contract | Make Contract queries/commands `public record` |

## Namespaces Reference

```csharp
// Contract query/command
using Shared.Contract.CQRS;

// MediatR sender
using MediatR;

// Result pattern
using Shared.Contract.ResultPattern;

// Module B Contract DTOs
using ModuleB.Contract.Dtos;
using ModuleB.Contract.Features.{Entity}.{Action};
```

(End of file - total 257 lines)