namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartInfoForCoupon
    {
        public IList<CartItemForCoupon> Items { get; set; } = new List<CartItemForCoupon>();
    }
}