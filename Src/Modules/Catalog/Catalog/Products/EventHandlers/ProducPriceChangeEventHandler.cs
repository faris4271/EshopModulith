using Catalog.Products.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandlers
{
    public class ProducPriceChangeEventHandler(ILogger<ProductPriceChangeEvent> logger, IBus bus) : INotificationHandler<ProductPriceChangeEvent>
    {
        public async Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);





        }
    }
}
