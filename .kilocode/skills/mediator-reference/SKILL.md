---
name: mediator-reference
description: MediatR library patterns and interfaces for EshopModulith. This project uses the MediatR library with custom CQRS interfaces. Reference when implementing commands, queries, and handlers.
user-invocable: false
---

# MediatR Reference

⚠️ **EshopModulith uses the `MediatR` library with custom interfaces from `Shared.Contract.CQRS`.**

While the project uses the MediatR library for dispatching, it defines its own `ICommand` and `IQuery` interfaces to maintain a consistent CQRS pattern across modules.

## Interface Comparison

| Purpose | ✅ EshopModulith (Use This) | ❌ Standard MediatR (Don't Use) |
|---------|----------------------------|--------------------------------|
| Command | `ICommand<TResponse>` | `IRequest<TResponse>` |
| Query | `IQuery<TResponse>` | `IRequest<TResponse>` |
| Command Handler | `ICommandHandler<TCommand, TResponse>` | `IRequestHandler<TRequest, TResponse>` |
| Query Handler | `IQueryHandler<TQuery, TResponse>` | `IRequestHandler<TRequest, TResponse>` |

## Command Pattern

```csharp
// ✅ Correct - EshopModulith
public sealed record CreateUserCommand(string Email, string Name) : ICommand<Guid>;

public sealed class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct)
    {
        // Implementation
        return Guid.NewGuid();
    }
}
```

## Query Pattern

```csharp
// ✅ Correct - EshopModulith
public sealed record GetUserQuery(Guid Id) : IQuery<UserDto>;

public sealed class GetUserHandler : IQueryHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery query, CancellationToken ct)
    {
        // Implementation
        return new UserDto(query.Id, "Test User");
    }
}
```

## Key Differences

| Aspect | EshopModulith | Standard MediatR |
|--------|----------------|------------------|
| Return type | `Task<T>` | `Task<T>` |
| Interfaces | Custom (`ICommand`, `IQuery`) | Standard (`IRequest`) |
| Dispatcher | `ISender` | `IMediator` |

## Sending Commands/Queries

```csharp
// In endpoint
public sealed class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users", async (
            CreateUserCommand command,
            ISender sender, 
            CancellationToken ct) => 
        {
            var result = await sender.Send(command, ct);
            return TypedResults.Created($"/users/{result}");
        })
        .AllowAnonymous();
    }
}
```

## Registration

```csharp
// In Program.cs
builder.Services.AddMediatRWithAssemblies();
```

## Common Errors

### Error: `IRequest<T>` not found
**Cause:** Using standard MediatR interface instead of project's custom CQRS interfaces.
**Fix:** Change to `ICommand<T>` or `IQuery<T>` from `Shared.Contract.CQRS`.

### Error: `IRequestHandler<T,R>` not found
**Cause:** Using standard MediatR interface.
**Fix:** Change to `ICommandHandler<T,R>` or `IQueryHandler<T,R>`.

### Error: Handler return type mismatch
**Cause:** Using `ValueTask<T>` instead of `Task<T>`.
**Fix:** Change handler return type to `Task<T>`.

## Namespaces

```csharp
// ✅ Correct for CQRS Interfaces
using Shared.Contract.CQRS;

// ✅ Correct for MediatR dispatching
using MediatR;
```

(End of file - total 132 lines)
