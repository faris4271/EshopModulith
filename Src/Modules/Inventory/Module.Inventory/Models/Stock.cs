using Shared.DDD;

namespace Module.Inventory.Models
{
    public class Stock : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }


        public Guid WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public int Quantity { get; set; }

        public int ReservedQuantity { get; set; }


        public static Stock Create(Guid productId, Guid warehouseId, int quantity = 0, int reservedQuantity = 0)
        {
            return new Stock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                ReservedQuantity = reservedQuantity
            };
        }
    }
}
