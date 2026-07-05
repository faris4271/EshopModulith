using Shared.Eventing.Contract;

namespace CatalogContract.IntegrationEvents
{
    public record UpdateProductStockQuantityIntegrationEvent(
        Guid Id,
        DateTime OccurredOnUtc,
        string CorrelationId,
        string Source,
        Guid productId,
        int adjustedQuantity,
        int newQuantity

        ) : IIntegrationEvent;

}
