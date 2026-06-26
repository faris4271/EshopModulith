using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductTemplet.CreatProductTemplet
{
    public record CreatProductTempletCommand(CreatProductTempletDto TempletDto) : ICommand;

}
