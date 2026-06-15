namespace Eshop.Module.Basket.Services
{
    public class CartInfoForCoupon
    {
        public IList<CartItemForCoupon> Items { get; set; } = new List<CartItemForCoupon>();
    }
}