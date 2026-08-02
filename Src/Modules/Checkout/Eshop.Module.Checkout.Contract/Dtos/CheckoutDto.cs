using CatalogContract.Dtos;
using System;
using System.Collections.Generic;


namespace Eshop.Module.Checkout.Contract.Dtos
{
    public record CheckoutDto
    {

        public Guid Id { get; set; }

        public bool LockedOnCheckout { get; set; }

        public string CouponCode { get; set; }

        public decimal SubTotal { get; set; }

        public string SubTotalString { get; set; }

        public decimal Discount { get; set; }

        public string DiscountString { get; set; }

        public string CouponValidationErrorMessage { get; set; }

        public bool IsProductPriceIncludeTax { get; set; }

        public decimal? TaxAmount { get; set; }

        public string OrderNote { get; set; }

        public string TaxAmountString { get; set; }


        public decimal? ShippingAmount { get; set; }

        public string ShippingAmountString { get; set; }



        public decimal SubTotalWithDiscount
        {
            get
            {
                return SubTotal - Discount;
            }
        }

        public decimal SubTotalWithDiscountWithoutTax
        {
            get
            {
                if (IsProductPriceIncludeTax)
                {
                    return SubTotalWithDiscount - TaxAmount ?? 0;
                }

                return SubTotalWithDiscount;
            }
        }

        public decimal OrderTotal
        {
            get
            {
                if (IsProductPriceIncludeTax)
                {
                    return SubTotal + (ShippingAmount ?? 0) - Discount;
                }

                return SubTotal + (TaxAmount ?? 0) + (ShippingAmount ?? 0) - Discount;
            }
        }

        public string OrderTotalString { get; set; }

        public IList<CheckoutItemDto> Items { get; set; } = new List<CheckoutItemDto>();

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
