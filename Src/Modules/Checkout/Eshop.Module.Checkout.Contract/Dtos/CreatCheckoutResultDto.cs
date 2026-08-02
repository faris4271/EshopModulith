using System;

namespace Eshop.Module.Checkout.Services
{
    public record CreatCheckoutResultDto(Guid Id, Guid UserId, bool IsProductPriceIncludeTax, string CouponCode, string ShippingMethod, DateTimeOffset CreatedOn);


}
