---
name: add-feature
description: Create a new API endpoint with Command, Handler, Validator, and Endpoint following EshopModulith vertical slice architecture. Use when adding any new feature, API endpoint, or business operation.
argument-hint: [ModuleName] [FeatureName]
---

# Add Feature

Create a complete vertical slice feature with all required files.

## File Structure

```
Src/Modules/{Module}/Features/{FeatureName}/
├── {Action}{Entity}Command.cs      # or Get{Entity}Query.cs
├── {Action}{Entity}Handler.cs
├── {Action}{Entity}Validator.cs    # Commands only
└── {Feature}Endpoints.cs           # ICarterModule
```

## Step 1: Create Command or Query

**For state changes (POST/PUT/DELETE):**
```csharp
public sealed record Create{Entity}Command(
    string Name,
    decimal Price) : ICommand<Create{Entity}Response>;
```

**For reads (GET):**
```csharp
public sealed record Get{Entity}Query(Guid Id) : IQuery<{Entity}Dto>;
```

## Step 2: Create Handler

```csharp
public sealed class Create{Entity}Handler(
    IGenericeRepository<{Entity}, {Module}DbContext> repository) : ICommandHandler<Create{Entity}Command, Create{Entity}Response>
{
    public async Task<Create{Entity}Response> Handle(
        Create{Entity}Command command,
        CancellationToken ct)
    {
        var entity = {Entity}.Create(command.Name, command.Price);
        await repository.AddAsync(entity, ct);
        return new Create{Entity}Response(entity.Id);
    }
}
```

## Step 3: Create Validator (Commands Only)

```csharp
public sealed class Create{Entity}Validator : AbstractValidator<Create{Entity}Command>
{
    public Create{Entity}Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

## Step 4: Create Endpoint

```csharp
public sealed class {Feature}Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/v1/{module}/{entity}", async (
            Create{Entity}Command command,
            ISender sender,
            CancellationToken ct) => TypedResults.Created(
                $"/api/v1/{module}/{entity}/{(await sender.Send(command, ct)).Id}"))
        .WithName(nameof(Create{Entity}Command))
        .WithSummary("Create a new {entity}")
        .AllowAnonymous();
    }
}
```

## Step 5: Add DTOs to Contracts

In `Src/Modules/{Module}/{Module}.Contract/`:

```csharp
public sealed record Create{Entity}Response(Guid Id);
public sealed record {Entity}Dto(Guid Id, string Name, decimal Price);
```

## Step 6: Wire Endpoint in Module

Carter modules are automatically discovered. No manual wiring in `MapEndpoints` is needed if `ICarterModule` is implemented.

## Step 7: Verify

```bash
dotnet build EshopModulith.slnx  # Must be 0 warnings
dotnet test EshopModulith.slnx
```

## Checklist

- [ ] Command/Query uses `ICommand<T>` or `IQuery<T>` from Shared.Contract
- [ ] Handler uses `ICommandHandler<T,R>` or `IQueryHandler<T,R>`
- [ ] Handler returns `Task<T>` (NOT `ValueTask<T>`)
- [ ] Validator exists for commands
- [ ] Endpoint implements `ICarterModule` and uses `.AllowAnonymous()`
- [ ] Endpoint has `.WithName()` and `.WithSummary()`
- [ ] DTOs in Contract project, not internal
- [ ] Build passes with 0 warnings

(End of file - total 116 lines)
