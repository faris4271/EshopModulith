using Shared.Eventing.Contract;

namespace CatalogContract.IntegrationEvents
{


    public record ProductCreatedIntegrationEvent(
        Guid Id,
        DateTime OccurredOnUtc,
        string CorrelationId,
        string Source,
        Guid ProductId,
       string Sku
    ) : IIntegrationEvent;

}
