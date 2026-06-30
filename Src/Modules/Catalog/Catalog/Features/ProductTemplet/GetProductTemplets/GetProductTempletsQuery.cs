using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductTemplet.GetProductTemplets
{
    public record GetProductTempletsQuery : IQuery<List<GetProductTempletsDto>>;

}
