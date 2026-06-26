using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductTemplet.UpdateProductTemplet
{
    public record UpdateProductTempletCommand(UpdateProductTempletDto TempletDto) : ICommand;
}