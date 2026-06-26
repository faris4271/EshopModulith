using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Products.Commands.CreatProduct
{
    public record CreatProductCommand(ProductForm ProductForm) : ICommand<Guid>;
}
