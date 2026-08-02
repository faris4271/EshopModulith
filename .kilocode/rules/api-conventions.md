---
paths:
  - "Src/Modules/**/Feature*/**/*"
  - "Src/Modules/**/*EndPoint*.cs"
  - "Src/Modules/**/*Endpoint*.cs"
---

# API Conventions

Rules for HTTP endpoints in EshopModulith. Endpoints are defined with **Carter** (`ICarterModule`) and dispatch via **MediatR** (`ISender`).

## Repository Reality (read this first)

- **Endpoints use `Results` (dynamic, `Microsoft.AspNetCore.Http.Results`), not `TypedResults`.** The previous "Always use `TypedResults` / never use `Results.Ok()`" guidance was the opposite of what the codebase actually does. Match the existing endpoints: `Results.NoContent()`, `Results.BadRequest()`, `Results.NotFound()`, `Results.Ok(...)`, `Results.Created(...)`.
- **Each feature slice ships its own `ICarterModule`** (the `*EndPoint.cs` file). Carter auto-discovers them per assembly — there is no central `{Module}Endpoints` aggregator and no `group.MapXEndpoint()` wiring (see [modules.md](modules.md) → "Wiring a Module").
- **There is no enforced `/api/v1/{module}/...` route convention.** Current routes are `"/api/{resource}"` style (`/api/vendors`, `/api/vendors/{id}`, `/api/vendors/grid`) and even vary in casing (`Catalog` uses `"Api/brands"`). When adding endpoints, **match the module's existing route style**. Recommended for new modules: lowercase `~/api/{module}/{entity}` — but do not retroactively rewrite existing routes.

## Endpoint shape (canonical, from Identity `Vendors`)

```csharp
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors.DeleteVendor;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.DeleteVendor;

internal sealed class DeleteVendorEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/vendors/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteVendorCommand(id));
            return result.Match(
                _   => Results.NoContent(),       // success branch (discard the returned Guid)
                err => Results.NotFound());        // failure branch
        })
        .WithTags("Vendors")
        .WithName(nameof(DeleteVendorCommand))
        .WithSummary("Delete a vendor")
        .AllowAnonymous();
    }
}
```

### Required / recommended metadata

| Chain method | Status | Purpose |
|---|---|---|
| `.WithTags("{Resource}")` | **required** — every endpoint has it | OpenAPI grouping |
| `.WithName(nameof(CommandOrQuery))` | recommended | unique route id (newer endpoints add it) |
| `.WithSummary("…")` | recommended | OpenAPI description |
| `.AllowAnonymous()` | recommended | no permission system enforced end-to-end yet |

Older endpoints (e.g. `CreatVendorEndPoint`, the grid endpoint) omit `.WithName`/`.WithSummary`/`.AllowAnonymous`. Add them on new work; don't churn existing endpoints just to back-fill.

## Result handling on the wire

Handlers return `Result` / `Result<T>` (see [modules.md](modules.md)). Convert to an HTTP response with one of:

```csharp
// 1. result.Match(onSuccess, onFailure) — preferred; onSuccess gets the value (if any)
return result.Match(_ => Results.NoContent(), err => Results.NotFound());

// 2. For query → Result<TDto>:
return result.Match(Results.Ok, Results.BadRequest);

// 3. Ternary on result.IsSuccess (also seen in the wild):
return result.IsSuccess ? Results.NoContent() : Results.BadRequest();
```

`result.Match` overloads (extension methods on `Shared.Contract.ResultPattern`):
- `Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)` on `Result`
- `Match<TIn, TOut>(Func<TIn, TOut> onSuccess, Func<Result<TIn>, TOut> onFailure)` on `Result<TIn>`

## HTTP method → Result mapping

| Operation | Method | Typical success response | Failure → |
|-----------|--------|---------------------------|-----------|
| Create | `MapPost` | `Results.Created($"/api/{resource}/{id}", ...)` or `Results.NoContent()` | `Results.BadRequest(...)` |
| Read single | `MapGet` | `Results.Ok(dto)` | `Results.NotFound()` |
| Read list / grid | `MapGet`/`MapPost` | `Results.Ok(list)` | `Results.BadRequest(...)` |
| Update | `MapPut` | `Results.NoContent()` | `Results.NotFound()` / `Results.BadRequest(...)` |
| Delete | `MapDelete` | `Results.NoContent()` | `Results.NotFound()` |

Use [Asp.Versioning] only when a route is actually versioned — current code mostly isn't.

## Dispatching commands & queries

Always use `ISender` (MediatR). Commands/Queries are public `record`s (often in the module's Contract project):

```csharp
// Create — bind body to the command record
app.MapPost("/api/vendors", async (CreatVendorCommand command, ISender sender) =>
{
    var result = await sender.Send(command);
    return result.IsSuccess ? Results.NoContent() : Results.BadRequest();
})
.WithTags("Vendors").WithSummary("Create a vendor").AllowAnonymous();

// Update — merge route id into the command
app.MapPut("/api/vendors/{id}", async (Guid id, UpdateVendorCommand command, ISender sender) =>
{
    var result = await sender.Send(command with { Id = id });
    return result.IsSuccess ? Results.NoContent() : Results.NotFound();
})
.WithTags("Vendors").WithName(nameof(UpdateVendorCommand)).AllowAnonymous();
```

## Query parameters

Simple filters: bind route/query params directly. Complex filters: the repo uses a **SmartTable** payload (a POST body deserialized to `SmartTableParam` from `Shared.Web.SmartTable`):

```csharp
app.MapPost("/api/vendors/grid", async (ISender sender, HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var bodyString = await reader.ReadToEndAsync();
    var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);
    var result = await sender.Send(new GetVenorsGridQuery(smartTableParam));
    return result.Match(Results.Ok, Results.BadRequest);
})
.WithTags("Vendors");
```

For `[AsParameters]`-style binding of many query params, prefer SmartTable where the surrounding module already uses it; otherwise plain route/query binding is fine.

## Route patterns (current state)

| Pattern | Example | Notes |
|---|---|---|
| `/api/{resource}` | `/api/vendors` | most common |
| `/api/{resource}/{id}` | `/api/vendors/{id}` | single item / update / delete |
| `/api/{resource}/grid` | `/api/vendors/grid` | paged/filtered list |
| `{Module}/{resource}` | `Api/brands` | Catalog-style; casing varies |

There is no global route prefix or `MapGroup` in use; each `ICarterModule` calls `app.MapX(...)` with a fully-qualified path. When adding to an existing module, copy its route prefix/casing exactly.

## Don'ts

- ❌ Don't return raw objects / `Results.Ok()` *without* a body typed to a DTO.
- ❌ Don't introduce `TypedResults` ("fixing" existing `Results` endpoints just churns code).
- ❌ Don't create a central `{Module}Endpoints : ICarterModule` that aggregates all routes — features own their endpoint.
- ❌ Don't add `.RequireAuthorization(...)` — no permission system is enforced at endpoints yet (auth is wired at the pipeline level in `Program.cs`; see `UseAuthentication`/`UseAuthorization` there).

---

For the CQRS records these endpoints dispatch, and how modules are wired/discovered, see [modules.md](modules.md). For the persistence behind handlers, see [persistence.md](persistence.md).