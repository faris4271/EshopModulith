---
paths:
  - "Src/Tests/**/*"
---

# Testing Rules

Rules for tests in EshopModulith.

## Test Organization

Test projects are currently planned for future implementation. When implemented, they will follow this structure:

```
Src/Tests/
├── Architecture.Tests/    # Layering enforcement (mandatory)
├── {Module}.Tests/        # Module-specific tests
└── Generic.Tests/         # Shared utilities
```

## Naming Conventions

| Type | Pattern |
|------|---------|
| Test class | `{ClassUnderTest}Tests` |
| Test method | `{Method}_{Scenario}_{ExpectedResult}` |
| Test file | Same as class name |

## Test Structure

Always use Arrange-Act-Assert:

```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccess()
{
    // Arrange
    var handler  = new BrandCommandHandler(/* repo, etc. */);
    var command  = new BrandCommand(/* … */);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert — handlers return Result<T>, not bare T
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBeEmpty();
}
```

## Required Tests

### For Handlers
- Happy path with valid input
- Edge cases (empty, null, boundary values)
- Repository interactions verified

### For Validators
- Each validation rule has a test
- Valid input passes
- Invalid input fails with correct property

### For Entities
- Valid construction (`new Entity(...)`) sets the expected fields
- Mutation methods (e.g. `Update(...)`) update fields predictably
- Domain event raising when applicable (clear `AddDomainEvent` calls)

> Note: there is no enforced `Entity.Create(...)` factory pattern in the framework — match what the aggregate already does (e.g. `Brand` accepts its params via a public constructor).

## Libraries

- **xUnit** - Test framework
- **FluentAssertions** - `.Should()` assertions
- **Moq** - `Mock<T>` for dependencies

## Architecture Tests

Architecture tests in `Architecture.Tests/` are mandatory and enforce:
- Module boundary isolation
- No cross-module internal dependencies
- Handlers/validators are sealed
- Contracts don't depend on implementations

These should run on every build and PR.
