---
name: add-feature
description: Add a vertical-slice feature (command/query + handler + validator + endpoint) to an existing FSH module. Use when adding an API endpoint or business operation to a module that already exists.
argument-hint: [ModuleName] [Area] [FeatureName]
---

# EshopModulith Add Feature

A feature is a vertical slice **split across two projects**: the request/response types live in the
module's `.Contracts` project (public API); the handler, validator, and endpoint live in the runtime
project. Full conventions: `.agents/rules/api-conventions.md`.

## Result Pattern (used everywhere)

This codebase uses `Shared.Contract.ResultPattern.Result<T>` — **never throw for business failures**.
Handlers return `Result<T>` (or `Result` for commands with no payload). Endpoints use `.Match(...)`
to translate to status codes.

```csharp
// Success cases
return Result.Success(entity.Id);                    // Result<Guid>
return Result.Success(dto);                          // Result<TDto>
return Result.Success();                             // Result (no payload)

// Failure cases (use Error factory methods)
return Result.Failure<string>(Error.NotFound("Entity.NotFound", $"Entity {id} not found."));
return Result.Failure<Dto>(Error.Conflict("Entity.Exists", "Name already taken."));
return Result.Failure(Error.Unauthorized("Auth.Required", "Please log in."));
return Result.Failure(Error.Failure("Operation.Failed", ex.Message));  // generic failure
```

Endpoints translate with `ResultExtensions.Match`:

```csharp
// The Match extension passes the Error object (not Result) to the failure handler
// Use error.Type to map to appropriate HTTP status code
var result = await mediator.Send(command, ct);
return result.Match(
    id => Results.Created($"/api/v1/{entities}/{id}", id),
    error => error.Type == ErrorType.NotFound
        ? Results.NotFound(new { Error = error.Description })
        : error.Type == ErrorType.Conflict
            ? Results.Conflict(new { Error = error.Description })
            : error.Type == ErrorType.Unauthorized
                ? Results.Unauthorized()
                : error.Type == ErrorType.Forbidden
                    ? Results.Forbid()
                    : Results.BadRequest(new { Error = error.Description }));
```

Error properties: `error.Code`, `error.Description` (NOT `.Message`), `error.Type` (`NotFound`, `Conflict`, `Unauthorized`, `Forbidden`, `Failure`, `Problem`, `Validation`).

## Layout (real)

```
src/Modules/{X}/Modules.{X}.Contracts/v1/{Area}/{Feature}Command.cs   # ICommand<T>/IQuery<T>
src/Modules/{X}/Modules.{X}.Contracts/Dtos/{Entity}Dto.cs             # response DTOs (if any)
src/Modules/{X}/Modules.{X}/Features/v1/{Area}/{Feature}/
├── {Feature}CommandHandler.cs    # public sealed, injects the DbContext directly
├── {Feature}CommandValidator.cs  # required for commands + paginated queries
└── {Feature}Endpoint.cs          # internal static extension
```

## Step 1 — Command/Query (Contracts project)

`Mediator` interfaces (`using Mediator;`). Records. A create command can return the raw `Guid`.

```csharp
namespace FSH.Modules.{X}.Contracts.v1.{Area};

public sealed record Create{Entity}Command(string Name, decimal PriceAmount, string PriceCurrency)
    : ICommand<Guid>;
```

Read/list DTOs go in `Modules.{X}.Contracts/Dtos/`. Paginated queries return `PagedResponse<T>`
(`FSH.Framework.Shared.Persistence`) — see `query-patterns`.

## Step 2 — Handler (runtime `Features/`) — inject the DbContext, NOT a repository

There is **no generic `IRepository<T>`**. Inject the module's `{X}DbContext`. `public sealed`, primary
ctor, `ValueTask<Result<T>>`, `.ConfigureAwait(false)`, guard first. Tenant/audit fields are auto-stamped — only
inject `ICurrentUser` if you need the acting user (`GetUserId()` / `GetTenant()`).

```csharp
public sealed class Create{Entity}CommandHandler({X}DbContext dbContext)
    : ICommandHandler<Create{Entity}Command, Guid>
{
    public async ValueTask<Result<Guid>> Handle(Create{Entity}Command command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // validation: duplicate check
        if (await dbContext.{Entities}.AnyAsync(e => e.Name == command.Name, cancellationToken))
            return Result.Failure<Guid>(Error.Conflict("{Entity}.NameExists", "Name already exists."));

        var entity = {Entity}.Create(command.Name, new Money(command.PriceAmount, command.PriceCurrency));
        dbContext.{Entities}.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return Result.Success(entity.Id);
    }
}
```

- **Throw** `NotFoundException` / `CustomException` only for **truly exceptional** cases (infra failures, null refs).
- **Return `Result.Failure`** for expected business outcomes: not found, conflict, validation failure, unauthorized.

## Step 3 — Validator (required; same folder)

```csharp
public sealed class Create{Entity}CommandValidator : AbstractValidator<Create{Entity}Command>
{
    public Create{Entity}CommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PriceCurrency).NotEmpty().Length(3);
    }
}
```

`Architecture.Tests` fails the build if a command/paginated-query handler has no `{Name}Validator`.

## Step 4 — Endpoint (same folder)

```csharp
public static class Create{Entity}Endpoint
{
    internal static RouteHandlerBuilder MapCreate{Entity}Endpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/{entities}",
                async (Create{Entity}Command command, IMediator mediator, CancellationToken ct) =>
                {
                    var result = await mediator.Send(command, ct);
                    return result.Match(
                        id => Results.Created($"/api/v1/{entities}/{id}", id),
                        error => error.Type == ErrorType.NotFound
                            ? Results.NotFound(new { Error = error.Description })
                            : error.Type == ErrorType.Conflict
                                ? Results.Conflict(new { Error = error.Description })
                                : error.Type == ErrorType.Unauthorized
                                    ? Results.Unauthorized()
                                    : error.Type == ErrorType.Forbidden
                                        ? Results.Forbid()
                                        : Results.BadRequest(new { Error = error.Description }));
                })
            .WithName("Create{Entity}")
            .WithSummary("Create a {entity}")
            .RequirePermission({X}Permissions.{Entities}.Create)
            .WithIdempotency();   // on replay-safe POSTs
}
```

## Step 5 — Wire it in `{X}Module.MapEndpoints`

```csharp
group.MapCreate{Entity}Endpoint();   // group = endpoints.MapGroup("api/v{version:apiVersion}/{x}") …
```

## Step 6 — Verify

```bash
dotnet build src/FSH.Starter.slnx          # 0 warnings (TreatWarningsAsErrors)
dotnet test src/Tests/{X}.Tests            # + add a handler/validator test (see testing-guide)
```

## Checklist

- [ ] Command/Query in the **Contracts** project (`using Mediator;`), DTOs in `Contracts/Dtos/`
- [ ] Handler `public sealed`, injects `{X}DbContext` (no repository), `ValueTask<Result<T>>` + `.ConfigureAwait(false)`
- [ ] Handler returns `Result.Success(...)` / `Result.Failure(Error.NotFound|Conflict|Unauthorized|Forbidden|Failure(...))`
- [ ] `{Name}Validator` exists
- [ ] Endpoint `internal static …Map{Feature}Endpoint`, uses `result.Match(...)` with error type mapping, `.RequirePermission(...)`, `.WithName/.WithSummary`
- [ ] Wired in `{X}Module.MapEndpoints`
- [ ] Build 0 warnings; test added
