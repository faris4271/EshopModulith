using CatalogContract.IntegrationEvents;
using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Eventing.Contract;

namespace Module.Inventory.EventHandler
{
    public class ProductCreatedEventHandler(
        IGenericeRepository<Warehouse, InventoryDbContext> _warhouseRepository,
        IGenericeRepository<Stock, InventoryDbContext> _stockRepository) : IIntegrationEventHandler<ProductCreatedIntegrationEvent>
    {
        public async Task HandleAsync(ProductCreatedIntegrationEvent @event, CancellationToken ct = default)
        {
            try
            {
                var queryWarehouse = await _warhouseRepository.Query();
                var defaultWarehouse = queryWarehouse.FirstOrDefault();

                var stock = Stock.Create(@event.ProductId, defaultWarehouse.Id);

                await _stockRepository.AddAsync(stock);

                await _stockRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}