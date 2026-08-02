using System;

namespace Eshop.Module.Checkout.Contract.Dtos
{
    public record CartItemToCheckoutDto(
        Guid ProductId,
        int Quantity

    );

}
