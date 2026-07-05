using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Module.Inventory.Models
{
    public class StockHistory : EntityBase<Guid>, IAuditableEntity
    {
        public Guid ProductId { get; private set; }


        public Guid WarehouseId { get; private set; }

        public Warehouse Warehouse { get; private set; }

        public long AdjustedQuantity { get; private set; }

        [StringLength(1000)]
        public string Note { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        public string? CreatedById { get; private set; }

        public DateTimeOffset? LatestUpdatedOn { get; private set; }

        public string? LatestUpdatedById { get; private set; }

        public static StockHistory Create(Guid productId, Guid warehouseId, long adjustedQuantity, string note)
        {
            return new StockHistory
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                AdjustedQuantity = adjustedQuantity,
                Note = note,
                CreatedOn = DateTimeOffset.UtcNow
            };

        }

        public void Update(long adjustedQuantity, string note)
        {
            AdjustedQuantity = adjustedQuantity;
            Note = note;
            LatestUpdatedOn = DateTimeOffset.UtcNow;
        }
    }
}
