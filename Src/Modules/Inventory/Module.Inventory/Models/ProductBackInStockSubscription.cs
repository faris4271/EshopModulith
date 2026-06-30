

using Shared.DDD;

namespace Module.Inventory.Models
{
    public class ProductBackInStockSubscription : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }

        public string CustomerEmail { get; set; }
    }
}
