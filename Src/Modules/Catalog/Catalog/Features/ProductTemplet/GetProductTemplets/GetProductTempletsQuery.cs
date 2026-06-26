using Shared.Contract.CQRS;
using SimplCommerce.Module.Catalog.Areas.Catalog.Controllers;

namespace Catalog.Features.ProductTemplet.GetProductTemplets
{
    public record GetProductTempletsQuery : IQuery<List<GetProductTempletsDto>>;

}
