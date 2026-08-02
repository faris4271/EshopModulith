namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartDto
    {



        public string CouponCode { get; set; }

        public decimal SubTotal { get; set; }

        public string SubTotalString { get; set; }

        public decimal Discount { get; set; }

        public string DiscountString { get; set; }

        public string CouponValidationErrorMessage { get; set; }

        public decimal SubTotalWithDiscount
        {
            get
            {
                return SubTotal - Discount;
            }
        }

        public string SubTotalWithDiscountString { get; set; }

        public IList<CartItemDto> Items { get; set; } = new List<CartItemDto>();

        public bool IsValid
        {
            get
            {
                foreach (var item in Items)
                {
                    if (!item.IsProductAvailabeToOrder)
                    {
                        return false;
                    }

                    if (item.ProductStockTrackingIsEnabled && item.ProductStockQuantity < item.Quantity)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
