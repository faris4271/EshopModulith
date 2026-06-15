using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Module.Inventory.Models
{
    public class StockHistory : EntityBase<Guid>, IAuditableEntity
    {
        public long ProductId { get; set; }


        public long WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public long AdjustedQuantity { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string? CreatedById { get; set; }

        public DateTimeOffset? LatestUpdatedOn { get; set; }

        public string? LatestUpdatedById { get; set; }
    }
}
