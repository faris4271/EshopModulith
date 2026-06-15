using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductPrices.UpdateProductPrice
{
    public record UpdateProductPriceCommand(ProductPriceItemDto ProductPriceItem) : ICommand;

}
