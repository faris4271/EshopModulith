using Shared.DDD;

namespace Module.Inventory.Models.Events
{

    public record StockQuantityChangedDomainEvent : DomainEvent
    {

        public Guid ProductId { get; init; }
        public int AdjustedQuantity { get; init; }
        public int NewQuantity { get; init; }
        public StockQuantityChangedDomainEvent(
            Guid eventId,
            DateTimeOffset occurredOnUtc,
            Guid productId,
            int adjustedQuantity,
            int newQuantity,
            string? correlationId = null)
            : base(eventId, occurredOnUtc, correlationId)
        {
            ProductId = productId;
            AdjustedQuantity = adjustedQuantity;
            NewQuantity = newQuantity;
        }

        public static StockQuantityChangedDomainEvent Create(Guid productId, int adjustedQuantity, int newQuantity)
            => new(Guid.NewGuid(), DateTimeOffset.UtcNow, productId, adjustedQuantity, newQuantity);
    }
}