namespace Eshop.Module.Basket.Contract.Dtos
{
    public record CartQuantityUpdateDto
    {
        public Guid cartItemId { get; init; }

        public int Quantity { get; init; }


    }
}
