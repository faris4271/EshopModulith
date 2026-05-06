# EshopModulith — AI Assistant Guide

> Modular Monolith · CQRS · .NET

## Quick Start

```bash
dotnet build EshopModulith.slnx              # Build (0 warnings required)
dotnet test EshopModulith.slnx               # Run tests
```

## Project Layout

```
Src/
├── Api/                    # Entry point (API)
├── BuildingBlocks/         # Shared libraries (Shared.*)
└── Modules/                # Business modules
    ├── Catalog/            # Catalog module
    ├── Identity/           # Identity module
    ├── Basket/             # Basket module
    └── ...                 # Other business modules
```

## The Pattern

Every feature = vertical slice:

```
Src/Modules/{Module}/{Project}/Features/{Feature}/
├── {Action}{Entity}Command.cs      # ICommand<T>
├── {Action}{Entity}Handler.cs      # ICommandHandler<T,R>
├── {Action}{Entity}Validator.cs    # AbstractValidator<T>
└── {Action}{Entity}Endpoint.cs     # ICarterModule implementation
```

## Critical Rules

| ⚠️ Rule | Why |
|---------|-----|
| Use **MediatR** | Standard CQRS library |
| `ICommand<T>` / `IQuery<T>` | Standard command/query interfaces |
| `Task<Result<T>>` return type | Performance optimization |
| Every command needs validator | FluentValidation, no exceptions |
| Zero build warnings | Quality standard |

## Example: Create Feature

```csharp
// Command
public sealed record CreateProductCommand(string Name, decimal Price) 
    : ICommand<Result>;

// Handler
public sealed class CreateProductHandler(IGenericRepository<Product> repo) 
    : ICommandHandler<CreateProductCommand, Result>
{
    public async Task<Result> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = Product.Create(cmd.Name, cmd.Price);
        await repo.AddAsync(product, ct);
        return Result.Success();
    }
}

// Validator
public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

// Endpoint
public sealed class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/", async (CreateProductCommand cmd, ISender sender, CancellationToken ct) =>
            {
                var result=await sender.Send(cmd, ct);

                result.Mach(Results.Created,Result.BadRequest)
            }))
        .WithName(nameof(CreateProductCommand))
        .WithSummary("Create a new product");
    }
}
```

## Architecture

- **Pattern:** Modular Monolith
- **CQRS:** MediatR (commands/queries)
- **Return Pattern:** `Task<Result>` using `Shared.Contract.ResultPattern`
- **Persistence:** Generic repository pattern (`IGenericRepository`)
- **Endpoints:** Carter for route organization

## Before Committing

```bash
dotnet build EshopModulith.slnx  # Must pass with 0 warnings
dotnet test EshopModulith.slnx   # All tests must pass
```
