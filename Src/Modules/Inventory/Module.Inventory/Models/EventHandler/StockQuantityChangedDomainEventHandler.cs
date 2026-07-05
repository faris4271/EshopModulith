using CatalogContract.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Module.Inventory.Models.Events;
using Shared.Eventing.OutBox;

namespace Module.Inventory.Models.EventHandler
{
    internal class StockQuantityChangedDomainEventHandler(IOutBoxStore _outBoxStore, ILogger<StockQuantityChangedDomainEventHandler> _logger) : INotificationHandler<StockQuantityChangedDomainEvent>
    {
        public async Task Handle(StockQuantityChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling StockQuantityChangedDomainEvent for ProductId:" +
                " {ProductId}, AdjustedQuantity: {AdjustedQuantity}, NewQuantity: {NewQuantity}",
                notification.ProductId, notification.AdjustedQuantity, notification.NewQuantity);

            await _outBoxStore.AddAsync(new UpdateProductStockQuantityIntegrationEvent(
                  Guid.NewGuid(),
                  DateTime.UtcNow,
                  Guid.NewGuid().ToString(),
                  "InventoryModule",
                  notification.ProductId,
                  notification.AdjustedQuantity,
                  notification.NewQuantity
              ));
        }
    }
}
