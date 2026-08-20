namespace Eshop.Module.Basket.Contract.Dtos;

using System;

public class BasketItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public string PriceText { get; set; }
    public string OriginalPriceText { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}