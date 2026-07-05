using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.IntegrationEvents;
using Shared.Abstraction;
using Shared.Eventing.Contract;

namespace Catalog.Products.IntegrationEventHandler
{
    public class ProductStockQuantityChangeIntegrationEventHandler(IGenericeRepository<Product, CatalogDbContext> _productRepository) : IIntegrationEventHandler<UpdateProductStockQuantityIntegrationEvent>
    {
        public async Task HandleAsync(UpdateProductStockQuantityIntegrationEvent @event, CancellationToken ct = default)
        {
            var query = await _productRepository.Query();
            var product = query.Where(p => p.Id == @event.productId).FirstOrDefault();

            product.UpdateStock(@event.newQuantity);

            await _productRepository.SaveChangesAsync(ct);
        }
    }
}
