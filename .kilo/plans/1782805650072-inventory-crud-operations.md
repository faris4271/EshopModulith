# Inventory Module CRUD Operations Plan

## Goal
Add full CRUD operations (Create, Read All, Read By Id, Update, Delete) for **Warehouse** and **Stock** entities in the Inventory module, following the same CQRS + Carter endpoint pattern used in the Catalog module (e.g., Brands CRUD).

## Context
- **Catalog module pattern**: Command/Query records → Handler → Carter endpoint (`ICarterModule`), all under `Features/<Entity>/<Operation>/` folders
- **Shared infrastructure**: `IGenericeRepository<T, DbContext>`, `ICommand`/`IQuery` (MediatR-based), `Result`/`Result<T>` pattern, Carter for endpoints
- **Inventory currently has**: Models (Stock, Warehouse, StockHistory, ProductBackInStockSubscription), DbContext, EF configurations, event handlers, no CQRS features or API endpoints

## Prerequisite: Create InventoryContract Project

Create `Src/Modules/Inventory/Module.Inventory.Contract/` project (mirroring CatalogContract):

### Project file (`Module.Inventory.Contract.csproj`)
- Target: net10.0
- Package refs: Carter, MediatR (same as CatalogContract)
- Project refs: Shared.Contract, Shared.Eventing.Contract

### DTOs (in `Module.Inventory.Contract/Dtos/`)
- `CreateWarehouseDto` — Name (string), VendorId (Guid?), Address fields (Street, City, State, ZipCode, Country, Phone, PostalCode)
- `UpdateWarehouseDto` — same fields + Id (Guid)
- `GetWarehouseDto` — Id, Name (string), VendorId, Address (nested or flat)
- `CreateStockDto` — ProductId (Guid), WarehouseId (Guid), Quantity (int), ReservedQuantity (int)
- `UpdateStockDto` — Id (Guid), ProductId, WarehouseId, Quantity, ReservedQuantity
- `GetStockDto` — Id, ProductId, WarehouseId, Quantity, ReservedQuantity, WarehouseName (optional)

### Add to solution (`EshopModulith.slnx`)
- Under `/Src/Modules/Inventory/` folder

### Update `Module.Inventory.csproj`
- Add project reference to `Module.Inventory.Contract`

## Feature Implementation (in Module.Inventory project)

Follow the exact folder convention: `Features/<EntityPlural>/<Operation>/` with 3 files each (Command/Query, Handler, Endpoint).

### 1. Warehouse CRUD — `Features/Warehouses/`

| Operation | Folder | Files |
|-----------|--------|-------|
| Create | `CreateWarehouse/` | `CreateWarehouseCommand.cs`, `CreateWarehouseCommandHandler.cs`, `CreateWarehouseEndpoint.cs` |
| Get All | `GetWarehouses/` | `GetWarehousesQuery.cs`, `GetWarehousesQueryHandler.cs`, `GetWarehousesEndpoint.cs` |
| Get By Id | `GetWarehouseById/` | `GetWarehouseByIdQuery.cs`, `GetWarehouseByIdQueryHandler.cs`, `GetWarehouseByIdEndpoint.cs` |
| Update | `UpdateWarehouse/` | `UpdateWarehouseCommand.cs`, `UpdateWarehouseCommandHandler.cs`, `UpdateWarehouseEndpoint.cs` |
| Delete | `DeleteWarehouse/` | `DeleteWarehouseCommand.cs`, `DeleteWarehouseCommandHandler.cs`, `DeleteWarehouseEndpoint.cs` |

**API routes** (following catalog's `api/brands` pattern):
- `POST api/warehouses` — Create
- `GET api/warehouses` — Get all
- `GET api/warehouses/{id}` — Get by id
- `PUT api/warehouses/{id}` — Update
- `DELETE api/warehouses/{id}` — Delete
- All tagged with `"Warehouse"`

**Warehouse model changes** — Add a static `Create` factory method and an `Update` method to `Warehouse.cs` (matching Brand pattern), so the handler can construct/update the entity encapsulated.

### 2. Stock CRUD — `Features/Stocks/`

| Operation | Folder | Files |
|-----------|--------|-------|
| Create | `CreateStock/` | `CreateStockCommand.cs`, `CreateStockCommandHandler.cs`, `CreateStockEndpoint.cs` |
| Get All | `GetStocks/` | `GetStocksQuery.cs`, `GetStocksQueryHandler.cs`, `GetStocksEndpoint.cs` |
| Get By Id | `GetStockById/` | `GetStockByIdQuery.cs`, `GetStockByIdQueryHandler.cs`, `GetStockByIdEndpoint.cs` |
| Update | `UpdateStock/` | `UpdateStockCommand.cs`, `UpdateStockCommandHandler.cs`, `UpdateStockEndpoint.cs` |
| Delete | `DeleteStock/` | `DeleteStockCommand.cs`, `DeleteStockCommandHandler.cs`, `DeleteStockEndpoint.cs` |

**API routes:**
- `POST api/stocks` — Create
- `GET api/stocks` — Get all
- `GET api/stocks/{id}` — Get by id
- `PUT api/stocks/{id}` — Update
- `DELETE api/stocks/{id}` — Delete
- All tagged with `"Stock"`

**Stock model changes** — Add an `Update` method to `Stock.cs` for Quantity and ReservedQuantity.

## InventoryModule.cs Updates

Update `InventoryModule.cs` to match the Catalog module's DI registration pattern:
1. Register MediatR: `services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))`
2. Register `IGenericeRepository<,>` → `GenericeRepository<,>` (scoped)
3. Register `ISaveChangesInterceptor` for `DispachDomainEventInterceptor` and `AuditableEntityInterceptor`
4. Add interceptors to DbContext (like Catalog does)
5. Add `using Module.Inventory.Data;` and `using Shared.Abstraction;`
6. Add `using Shared.Data;` and `using Shared.Data.interceptores;`

## Migration

After changes, generate a new EF Core migration (though no schema changes are expected to existing tables — only app-layer changes). If Warehouse model gains factory/Update methods only, no migration needed.

## Validation

- Run `dotnet build` on the solution to verify compilation
- Verify all new endpoints are registered by checking Carter module discovery

## Out of Scope
- StockHistory CRUD (audit-trail entity, should be append-only)
- ProductBackInStockSubscription CRUD (subscription lifecycle is managed by the event handler)
- Validators (Catalog Brands don't use validators heavily — can be added later)
- Integration events for Warehouse/Stock changes
