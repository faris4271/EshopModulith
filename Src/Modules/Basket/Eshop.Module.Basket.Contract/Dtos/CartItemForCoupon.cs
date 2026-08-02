namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartItemForCoupon
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}