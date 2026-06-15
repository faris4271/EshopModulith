using Shared.DDD;

namespace Module.Inventory.Models
{
    public class Stock : EntityBase<Guid>
    {
        public long ProductId { get; set; }


        public long WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public int Quantity { get; set; }

        public int ReservedQuantity { get; set; }
    }
}
