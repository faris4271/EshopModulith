using Catalog.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Catalog.Products.EventHandlers
{
    internal class ProductCreatedEventHandler(ILogger<CreatProductEvent> logger) : INotificationHandler<CreatProductEvent>
    {
        public async Task Handle(CreatProductEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);






        }
    }
}
