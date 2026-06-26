
using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(Guid id, ProductForm ProductForm) : ICommand<Guid>;

}