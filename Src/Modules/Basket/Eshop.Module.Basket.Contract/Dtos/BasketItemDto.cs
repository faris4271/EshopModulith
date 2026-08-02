namespace Eshop.Module.Basket.Contract.Dtos
{
    public record BasketItemDto(
        Guid Id,
        Guid ProductId,
        int Quantity,
        decimal Price);

}
