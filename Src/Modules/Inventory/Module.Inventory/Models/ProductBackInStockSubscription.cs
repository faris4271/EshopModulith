

using Shared.DDD;

namespace Module.Inventory.Models
{
    public class ProductBackInStockSubscription : EntityBase<Guid>
    {
        public long ProductId { get; set; }

        public string CustomerEmail { get; set; }
    }
}
