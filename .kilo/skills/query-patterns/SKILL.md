---
name: query-patterns
description: Query patterns including pagination, search, filtering, and specifications for EshopModulith. Use when implementing GET endpoints that return lists or need filtering.
---

# Query Patterns

Reference for implementing queries with pagination, search, and filtering.

## Basic Paginated Query

```csharp
// Query
public sealed record Get{Entities}Query(
    string? Search,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedList<{Entity}Dto>>;

// Handler
public sealed class Get{Entities}Handler(
    IGenericeRepository<{Entity}, {Module}DbContext> repository) : IQueryHandler<Get{Entities}Query, PagedList<{Entity}Dto>>
{
    public async Task<PagedList<{Entity}Dto>> Handle(
        Get{Entities}Query query,
        CancellationToken ct)
    {
        var spec = new {Entity}SearchSpec(query.Search, query.PageNumber, query.PageSize);
        var entities = await repository.ListAsync(spec, ct);
        
        // Map to DTOs and return PagedList
        return new PagedList< {Entity}Dto>(
            entities.Select(e => e.ToDto()).ToList(),
            query.PageNumber,
            query.PageSize,
            await repository.CountAsync(spec, ct));
    }
}
```

## Specification Pattern

```csharp
public sealed class {Entity}SearchSpec : Specification<{Entity}>
{
    public {Entity}SearchSpec(string? search, int pageNumber, int pageSize)
    {
        Query
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => string.IsNullOrEmpty(search) ||
                        x.Name.Contains(search) ||
                        x.Description!.Contains(search))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
```

## Get Single Entity

```csharp
// Query
public sealed record Get{Entity}Query(Guid Id) : IQuery<{Entity}Dto>;

// Handler
public sealed class Get{Entity}Handler(
    IGenericeRepository<{Entity}, {Module}DbContext> repository) : IQueryHandler<Get{Entity}Query, {Entity}Dto>
{
    public async Task<{Entity}Dto> Handle(Get{Entity}Query query, CancellationToken ct)
    {
        var spec = new {Entity}ByIdSpec(query.Id);
        var entity = await repository.FirstOrDefaultAsync(spec, ct);

        return entity?.ToDto() ?? throw new NotFoundException($"{Entity} {query.Id} not found");
    }
}

// Specification
public sealed class {Entity}ByIdSpec : Specification<{Entity}>
{
    public {Entity}ByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
```

## Advanced Filtering

```csharp
public sealed record Get{Entities}Query(
    string? Search,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    DateTimeOffset? CreatedAfter,
    bool? IsActive,
    string? SortBy,
    bool SortDescending = false,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedList<{Entity}Dto>>;

public sealed class {Entity}FilterSpec : Specification<{Entity}>
{
    public {Entity}FilterSpec(Get{Entities}Query query)
    {
        Query
            // Search
            .Where(x => string.IsNullOrEmpty(query.Search) ||
                        x.Name.Contains(query.Search))

            // Filters
            .Where(x => !query.CategoryId.HasValue ||
                        x.CategoryId == query.CategoryId)
            .Where(x => !query.MinPrice.HasValue ||
                        x.Price >= query.MinPrice)
            .Where(x => !query.MaxPrice.HasValue ||
                        x.Price <= query.MaxPrice)
            .Where(x => !query.CreatedAfter.HasValue ||
                        x.CreatedAt >= query.CreatedAfter)
            .Where(x => !query.IsActive.HasValue ||
                        x.IsActive == query.IsActive);

        // Dynamic sorting
        ApplySorting(query.SortBy, query.SortDescending);
    }

    private void ApplySorting(string? sortBy, bool descending)
    {
        switch (sortBy?.ToLowerInvariant())
        {
            case "name":
                if (descending) Query.OrderByDescending(x => x.Name);
                else Query.OrderBy(x => x.Name);
                break;
            case "price":
                if (descending) Query.OrderByDescending(x => x.Price);
                else Query.OrderBy(x => x.Price);
                break;
            default:
                Query.OrderByDescending(x => x.CreatedAt);
                break;
        }
    }
}
```

## Endpoint Patterns

### List Endpoint

```csharp
public sealed class {Feature}Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/{module}/{entities}", async (
            [AsParameters] Get{Entities}Query query,
            ISender sender,
            CancellationToken ct) => TypedResults.Ok(await sender.Send(query, ct)))
        .WithName(nameof(Get{Entities}Query))
        .WithSummary("Get paginated list of {entities}")
        .AllowAnonymous();
    }
}
```

### Single Entity Endpoint

```csharp
public sealed class {Feature}Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/{module}/{entities}/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken ct) => TypedResults.Ok(await sender.Send(new Get{Entity}Query(id), ct)))
        .WithName(nameof(Get{Entity}Query))
        .WithSummary("Get {entity} by ID")
        .AllowAnonymous();
    }
}
```

## Response Types

```csharp
// In Contract project
public sealed record {Entity}Dto(
    Guid Id,
    string Name,
    decimal Price,
    string? Description,
    DateTimeOffset CreatedAt);

// PagedList<T> is from BuildingBlocks
// Returns: Items, PageNumber, PageSize, TotalCount, TotalPages
```

## Key Points

1. **Use specifications** - Don't write raw LINQ in handlers
2. **Soft delete filtering is automatic** - DeletedAt != null filtered out
3. **Use `[AsParameters]`** - For query parameters in endpoints
4. **Project to DTOs** - Never return entities directly

(End of file - total 187 lines)
