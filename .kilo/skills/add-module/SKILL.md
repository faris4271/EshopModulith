---
name: add-module
description: Create a new module (bounded context) with proper project structure, permissions, DbContext, and registration. Use when adding a new business domain that needs its own entities and endpoints.
argument-hint: [ModuleName]
---

# Add Module

Create a new bounded context with full project structure.

## When to Create a New Module

- Has its own domain entities
- Could be deployed independently
- Represents a distinct business domain

If it's just a feature in an existing domain, use `add-feature` instead.

## Project Structure

```
Src/Modules/{Name}/
├── Eshop.Module.{Name}/
│   ├── Eshop.Module.{Name}.csproj
│   ├── {Name}Endpoints.cs (ICarterModule)
│   ├── {Name}Permissions.cs
│   ├── {Name}DbContext.cs
│   ├── Entities/
│   │   └── {Entity}.cs
│   └── Features/v1/
│       └── {Feature}/
└── Eshop.Module.{Name}.Contract/
    ├── Eshop.Module.{Name}.Contract.csproj
    └── DTOs/
```

## Step 1: Create Projects

### Main Module Project
`Src/Modules/{Name}/Eshop.Module.{Name}/Eshop.Module.{Name}.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\BuildingBlocks\Shared\Shared.csproj" />
    <ProjectReference Include="..\..\BuildingBlocks\Shared.Persistence\Shared.Persistence.csproj" />
    <ProjectReference Include="..\..\BuildingBlocks\Shared.Web\Shared.Web.csproj" />
    <ProjectReference Include="..\Eshop.Module.{Name}.Contract\Eshop.Module.{Name}.Contract.csproj" />
  </ItemGroup>
</Project>
```

### Contracts Project
`Src/Modules/{Name}/Eshop.Module.{Name}.Contract/Eshop.Module.{Name}.Contract.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
</Project>
```

## Step 2: Implement Endpoints (Carter)

```csharp
public sealed class {Name}Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/{name}")
            .WithTags("{Name}");
        // Map feature endpoints here
    }
}
```

## Step 3: Add Permission Constants

```csharp
public static class {Name}Permissions
{
    public static class {Entities}
    {
        public const string View = "{Entities}.View";
        public const string Create = "{Entities}.Create";
        public const string Update = "{Entities}.Update";
        public const string Delete = "{Entities}.Delete";
    }
}
```

## Step 4: Create DbContext

```csharp
public sealed class {Name}DbContext(DbContextOptions<{Name}DbContext> options) : BaseDbContext(options)
{
    public DbSet<{Entity}> {Entities} => Set<{Entity}>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("{name}");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof({Name}DbContext).Assembly);
    }
}
```

## Step 5: Register in Program.cs

Create a DI extension method:
```csharp
public static class {Name}Extensions
{
    public static IServiceCollection Add{Name}Module(this IServiceCollection services)
    {
        services.AddDbContext<{Name}DbContext>();
        // Register repositories
        return services;
    }
}
```

In `Src/Api/Api/Program.cs`:
```csharp
builder.Services.Add{Name}Module();
```

## Step 6: Register in Carter Configuration

Add the module to the Carter configuration in `Src/Api/Api/Program.cs`:
```csharp
var {name}Assembly = typeof({Name}Module).Assembly;

builder.Services.AddCarter(configurator: config =>
{
    // Existing module registrations
    
    var {name}ModuleTypes = typeof({Name}Module).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();
    
    config.WithModules({name}ModuleTypes);
});
```

## Step 7: Use the Module in the Application

Add the module to the application pipeline in `Src/Api/Api/Program.cs`:
```csharp
app.Use{Name}();
```

## Step 8: Add to Solution

Create a DI extension method:
```csharp
public static class {Name}Extensions
{
    public static IServiceCollection Add{Name}Module(this IServiceCollection services)
    {
        services.AddDbContext<{Name}DbContext>();
        // Register repositories
        return services;
    }
}
```

In `Src/Api/Api/Program.cs`:
```csharp
builder.Services.Add{Name}Module();
```

## Step 6: Add to Solution

```bash
dotnet sln EshopModulith.slnx add Src/Modules/{Name}/Eshop.Module.{Name}/Eshop.Module.{Name}.csproj
dotnet sln EshopModulith.slnx add Src/Modules/{Name}/Eshop.Module.{Name}.Contract/Eshop.Module.{Name}.Contract.csproj
```

## Step 7: Reference from API

In `Src/Api/Api/Api.csproj`:
```xml
<ProjectReference Include="..\..\Modules\{Name}\Eshop.Module.{Name}\Eshop.Module.{Name}.csproj" />
```

## Step 8: Verify

```bash
dotnet build EshopModulith.slnx  # Must be 0 warnings
dotnet test EshopModulith.slnx
```

## Checklist

- [ ] Both projects created (main + contract)
- [ ] ICarterModule implemented for endpoints
- [ ] Permission constants defined
- [ ] DbContext created with proper schema
- [ ] Registered in Program.cs via extension method
- [ ] Added to solution file
- [ ] Referenced from Api project
- [ ] Build passes with 0 warnings

(End of file - total 176 lines)
