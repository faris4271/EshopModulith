# EshopModulith — E-Commerce Platform

> A production-ready **modular monolith** e-commerce platform built with **.NET**, **CQRS**, **MediatR**, and **Carter**.

![.NET Version](https://img.shields.io/badge/.NET-8%2B-blue)
![Architecture](https://img.shields.io/badge/Architecture-Modular%20Monolith-green)
![CQRS](https://img.shields.io/badge/Pattern-CQRS%20%2B%20MediatR-orange)
![Docker](https://img.shields.io/badge/Docker-Supported-2496ED)

---

## 🎯 Overview

**EshopModulith** is a comprehensive e-commerce platform designed as a modular monolith. It showcases industry best practices for building scalable, maintainable, and feature-rich business applications. With multiple modules (Catalog, Identity, Basket, Localization), it demonstrates clean architecture, CQRS patterns, and event-driven design.

### Key Features

✅ **Modular Architecture** - Loosely-coupled business modules with clear boundaries  
✅ **CQRS Pattern** - Command Query Responsibility Segregation using MediatR  
✅ **Product Catalog** - Browse, filter, and manage products with categories  
✅ **Shopping Basket** - Add items, manage cart, apply discount rules  
✅ **Identity Management** - User authentication, roles, and authorization  
✅ **Localization** - Multi-language support for products and content  
✅ **Result Pattern** - Standardized error handling and validation  
✅ **Docker Support** - Full Docker Compose setup for local development  
✅ **Event-Driven** - Integration events across modules using MassTransit  
✅ **Entity Framework Core** - PostgreSQL with migrations and interceptors  

---

## 📋 Table of Contents

- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Architecture](#architecture)
- [Module Documentation](#module-documentation)
- [Development Guidelines](#development-guidelines)
- [API Patterns](#api-patterns)
- [Running & Testing](#running--testing)
- [Docker Setup](#docker-setup)
- [Contributing](#contributing)

---

## 🚀 Quick Start

### Prerequisites

- **.NET 8 SDK** or higher
- **Docker & Docker Compose** - For PostgreSQL and Redis
- **Visual Studio 2022** or **VS Code**

### Setup & Run

```bash
# Clone the repository
git clone https://github.com/faris4271/EshopModulith.git
cd EshopModulith

# Restore dependencies
dotnet restore EshopModulith.slnx

# Build the solution (0 warnings required)
dotnet build EshopModulith.slnx

# Start services (PostgreSQL, Redis, etc.)
docker-compose up -d

# Run the API
dotnet run --project Src/Api/Api

# Run tests
dotnet test EshopModulith.slnx
```

The API will be available at:
- **API Base URL**: https://localhost:5001
- **Swagger UI**: https://localhost:5001/swagger
- **Health Check**: https://localhost:5001/health

---

## 📁 Project Structure

```
EshopModulith/
├── Src/
│   ├── Api/                          # API Entry Point
│   │   ├── Api/                      # Main ASP.NET Core API project
│   │   ├── Dockerfile               # Container image
│   │   └── Program.cs               # Bootstrapping & DI setup
│   │
│   ├── BuildingBlocks/              # Shared libraries & frameworks
│   │   ├── Shared.Abstraction/      # Interfaces & abstractions
│   │   ├── Shared.Persistence/      # EF Core base classes
│   │   ├── Shared.Contract/         # Result pattern, CQRS interfaces
│   │   ├── Shared.Eventing/         # Event bus & handlers
│   │   ├── Shared.Caching/          # Redis caching
│   │   ├── Shared.Mailing/          # Email services
│   │   ├── Shared.Web/              # Web middleware, auth
│   │   ├── Shared.Jobs/             # Background jobs
│   │   └── Shared.Storage/          # File storage
│   │
│   └── Modules/                     # Business feature modules
│       ├── Catalog/                 # Product catalog
│       │   ├── Catalog/             # Implementation
│       │   └── CatalogContract/     # Public API & DTOs
│       │
│       ├── Basket/                  # Shopping cart
│       │   ├── Eshop.Module.Basket/ # Implementation
│       │   └── Eshop.Module.Basket.Contract/ # Public API
│       │
│       ├── Identity/                # Authentication & users
│       │   ├── IdentityModule/      # Implementation
│       │   └── Module.Identity.Contract/ # Public API
│       │
│       ├── Localization/            # Multi-language support
│       │   ├── Eshop.Module.Localization/
│       │   └── Eshop.Module.Localization.Contract/
│       │
│       └── Core/                    # Core business logic
│           ├── Eshop.Module.Core/
│           └── EShop.Module.Core.Contract/
│
├── docker-compose.yml               # Local environment setup
├── docker-compose.override.yml      # Development overrides
├── .containers/                     # Container configurations
├── EshopModulith.slnx              # Solution file
└── README.md                        # This file
```

---

## 🏗️ Architecture

### Design Principles

| Pattern | Purpose |
|---------|---------|
| **Modular Monolith** | Single deployment with isolated modules |
| **CQRS** | Separate read/write operations for scalability |
| **MediatR** | Request/response mediator for loose coupling |
| **Result Pattern** | Consistent error handling without exceptions |
| **DDD** | Domain-driven entities and business logic |
| **Repository Pattern** | Data access abstraction |
| **Event-Driven** | MassTransit for inter-module communication |

### Module Communication Flow

```
┌─────────────────────────────────────────┐
│         ASP.NET Core API (Program.cs)    │
│    (Carter for route registration)       │
└────────────┬────────────────────────────┘
             │
    ┌────────┴────────┐
    ▼                 ▼
┌──────────┐    ┌──────────┐
│  Modules │    │ BuildingBlocks
├──────────┤    ├──────────┐
│ Catalog  │    │ Result   │
│ Basket   │    │ CQRS     │
│ Identity │    │ Eventing │
│ Core     │    │ Caching  │
└────┬─────┘    └─────────┘
     │
┌────▼──────────────────────┐
│  MediatR Pipeline         │
│  (Handlers & Validators)  │
└────┬──────────────────────┘
     │
┌────▼──────────────────────────┐
│  Repository Pattern            │
│  (EF Core + PostgreSQL)        │
└───────────────────────────────┘
```

---

## 📦 Module Documentation

### 1. **Catalog Module** 📦
**Location**: `Src/Modules/Catalog/`

Features:
- Product management & search
- Category hierarchy
- Product pricing & inventory
- Image/media management

Key Components:
- `CatalogModule.cs` - Module registration
- `Features/Products/` - CQRS commands & queries
- `Data/CatalogDbContext.cs` - Database context
- `Services/ProductPricingService` - Business logic

### 2. **Basket Module** 🛒
**Location**: `Src/Modules/Basket/`

Features:
- Shopping cart management
- Add/remove items
- Discount & coupon rules
- Cart persistence per user

Key Components:
- `BasketModule.cs` - Module registration
- `Services/CartService` - Cart operations
- `Models/CatalogRule` - Discount rules
- `Feature/Commands/AddToBasket` - Add to cart CQRS

### 3. **Identity Module** 🔐
**Location**: `Src/Modules/Identity/`

Features:
- User registration & authentication
- JWT token generation
- Role-based access control (RBAC)
- Password management

Key Components:
- `IdentityModule.cs` - Module registration
- `Services/JwtService` - Token generation
- `IdentityDbContext` - User data
- `Authorization/` - Permission logic

### 4. **Localization Module** 🌍
**Location**: `Src/Modules/Localization/`

Features:
- Multi-language support
- Translation management
- Locale-specific content

### 5. **Core Module** ⚙️
**Location**: `Src/Modules/Core/`

Features:
- Media management
- Shared domain models
- Common business logic

---

## 💻 Development Guidelines

### Feature Implementation Pattern

Every feature follows the **vertical slice** architecture:

```
Src/Modules/{Module}/{Project}/Features/{Feature}/
├── {Action}{Entity}Command.cs      # ICommand<Result<T>>
├── {Action}{Entity}Handler.cs      # ICommandHandler<T, Result>
├── {Action}{Entity}Validator.cs    # AbstractValidator<T>
├── {Action}{Entity}Endpoint.cs     # ICarterModule implementation
└── {Action}{Entity}Dto.cs          # Data transfer object
```

### Command/Query Example

```csharp
// Command Definition
public sealed record AddToBasketCommand(Guid ProductId, int Quantity)
    : ICommand<Result<CartDto>>;

// Handler
public sealed class AddToBasketCommandHandler(
    ICartService cartService,
    ICurrentUserService currentUserService)
    : ICommandHandler<AddToBasketCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(
        AddToBasketCommand cmd, 
        CancellationToken ct)
    {
        var userId = currentUserService.GetUserId();
        
        var result = await cartService.AddToCart(
            userId, 
            cmd.ProductId, 
            cmd.Quantity);

        if (!result.Success)
            return Result.Failure<CartDto>(
                Error.Failure(result.ErrorCode, result.ErrorMessage));

        var cart = await cartService.GetCartDetails(userId);
        return Result.Success(cart);
    }
}

// Validator
public sealed class AddToBasketValidator : AbstractValidator<AddToBasketCommand>
{
    public AddToBasketValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(1000).WithMessage("Quantity cannot exceed 1000");
    }
}

// Endpoint (Carter Module)
public sealed class AddToBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/basket/items", AddToBasket)
            .WithName(nameof(AddToBasketCommand))
            .WithOpenApi()
            .Produces<CartDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> AddToBasket(
        AddToBasketCommand cmd,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(cmd, ct);
        return result.Match(Results.Ok, Results.BadRequest);
    }
}
```

### Critical Rules

| Rule | Why |
|------|-----|
| Use **MediatR** (not custom mediator) | Industry standard CQRS library |
| Return `Task<Result<T>>` | Consistent error handling |
| Create **Validator** for every Command | FluentValidation prevents invalid operations |
| Implement **ICarterModule** for endpoints | Organized route registration |
| Zero build warnings | Quality standard enforced in CI |
| Use **Result Pattern** | Avoid exceptions for business logic |

### Code Standards

- Follow `Directory.Build.props` settings
- Use **primary constructors** for DI
- Keep features under 300 lines
- Abstract business logic to **Services**
- Use **DTOs** for API responses
- Apply **Result Pattern** consistently

---

## 🧪 Running & Testing

### Build & Compile

```bash
# Build with strict checks
dotnet build EshopModulith.slnx

# Build with verbose output
dotnet build EshopModulith.slnx -v d

# Check for warnings only
dotnet build EshopModulith.slnx --no-restore 2>&1 | grep -i warning
```

### Run Tests

```bash
# Run all tests
dotnet test EshopModulith.slnx

# Run specific test project
dotnet test Src/Tests/Catalog.Tests

# Run with coverage
dotnet test EshopModulith.slnx /p:CollectCoverage=true

# Watch mode
dotnet watch --project Src/Tests/Catalog.Tests test
```

### Run the API

```bash
# Development mode
dotnet run --project Src/Api/Api --configuration Debug

# Release mode
dotnet run --project Src/Api/Api --configuration Release

# With specific port
dotnet run --project Src/Api/Api -- --urls https://localhost:7001
```

---

## 🐳 Docker Setup

### Quick Start with Docker

```bash
# Start all services (PostgreSQL, Redis, API)
docker-compose up -d

# Stop services
docker-compose down

# View logs
docker-compose logs -f api

# Rebuild images
docker-compose up --build
```

### Docker Compose Services

The `docker-compose.yml` includes:

- **PostgreSQL** - Database (port 5432)
- **Redis** - Caching (port 6379)
- **API** - ASP.NET Core application (port 5001)

### Build Docker Image

```bash
# Build image
docker build -f Src/Api/Api/Dockerfile -t eshop-modulith:latest .

# Run container
docker run -p 5001:5001 eshop-modulith:latest

# Run with environment variables
docker run -e ConnectionStrings__Default=... eshop-modulith:latest
```

---

## 📝 Common Tasks

### Add a New Feature to Catalog

```bash
# 1. Create feature folder
mkdir -p Src/Modules/Catalog/Catalog/Features/Products/Commands/CreateProduct

# 2. Add files:
#    - CreateProductCommand.cs
#    - CreateProductHandler.cs
#    - CreateProductValidator.cs
#    - CreateProductEndpoint.cs

# 3. Register handler in MediatR config:
# (Already done via reflection in CatalogModule.cs)

# 4. Build and test
dotnet build EshopModulith.slnx
```

### Add a New Module

```bash
# 1. Create module structure
mkdir -p Src/Modules/YourModule/YourModule/{Features,Models,Services,Data}
mkdir -p Src/Modules/YourModule/YourModule.Contract/{Dtos,Services}

# 2. Create YourModule.csproj and YourModule.Contract.csproj

# 3. Implement YourModuleExtensions class:
public static class YourModuleExtensions
{
    public static IServiceCollection AddYourModule(this IServiceCollection services)
    {
        // Register services
        return services;
    }
}

# 4. Register in Program.cs (Src/Api/Api/Program.cs)
builder.Services.AddYourModule(builder.Configuration);
```

### Run Database Migrations

```bash
# Create a new migration
dotnet ef migrations add "AddNewTable" \
    --project Src/Modules/Catalog/Catalog \
    --context CatalogDbContext \
    --startup-project Src/Api/Api

# Apply migrations
dotnet ef database update \
    --project Src/Modules/Catalog/Catalog \
    --context CatalogDbContext
```

---

## 📚 Additional Resources

- **[CLAUDE.md](./CLAUDE.md)** - AI assistant guide with patterns
- **Docker Compose** - `docker-compose.yml` for local environment
- **Swagger/OpenAPI** - Auto-generated API documentation
- **.NET Docs** - https://learn.microsoft.com/en-us/dotnet/

---

## 🤝 Contributing

We welcome contributions! Please:

1. **Fork** the repository
2. **Create a feature branch**: `git checkout -b feature/your-feature`
3. **Follow the patterns** in this README
4. **Ensure zero warnings**: `dotnet build EshopModulith.slnx`
5. **Run tests**: `dotnet test EshopModulith.slnx`
6. **Submit a PR** with a clear description

---

## 📝 License

This project is open source and available under an appropriate license.

---

## 🎓 Learning Resources

- **CQRS Pattern** - Command Query Responsibility Segregation
- **MediatR** - Mediator pattern implementation for .NET
- **Result Pattern** - Functional error handling
- **Entity Framework Core** - ORM for .NET
- **Carter** - Route organization library for Minimal APIs
- **Docker & Compose** - Containerization and orchestration

---

## 💬 Support

- **Issues**: Report bugs or request features via [GitHub Issues](https://github.com/faris4271/EshopModulith/issues)
- **Discussions**: Ask questions in [GitHub Discussions](https://github.com/faris4271/EshopModulith/discussions)

---

## 🚀 Project Roadmap

- ✅ Core CQRS implementation
- ✅ Catalog module
- ✅ Basket/Cart system
- ✅ Identity & Authentication
- 🔄 Payment integration (in progress)
- 📋 Order management (planned)
- 📋 Admin dashboard (planned)
- 📋 Recommendations engine (planned)

---

**Built with ❤️ for e-commerce excellence**

*A modern, scalable approach to building production-ready applications.*
