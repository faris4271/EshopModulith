using Module.Inventory.Models.Events;
using Shared.DDD;

namespace Module.Inventory.Models
{
    public class Stock : Aggregate<Guid>
    {
        public Guid ProductId { get; set; }

        public string sku { get; set; }
        public Guid WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public int Quantity { get; set; }

        public int ReservedQuantity { get; set; }



        public static Stock Create(Guid productId, Guid warehouseId, string sku, int quantity = 0, int reservedQuantity = 0)
        {
            return new Stock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                sku = sku,
                Quantity = quantity,
                ReservedQuantity = reservedQuantity
            };
        }

        public void UpdateQuantity(int adjustedQuantity)
        {

            if (adjustedQuantity < 0 && Math.Abs(adjustedQuantity) > this.Quantity)
            {
                adjustedQuantity = -this.Quantity;
            }

            this.Quantity += adjustedQuantity;


            this.AddDomainEvent(StockQuantityChangedDomainEvent.Create(this.ProductId, adjustedQuantity, this.Quantity));
        }
    }
}
