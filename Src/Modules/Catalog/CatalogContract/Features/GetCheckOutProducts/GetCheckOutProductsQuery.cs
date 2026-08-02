using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace CatalogContract.Features.GetCheckOutProducts
{
    public record GetCheckOutProductsQuery(List<Guid> productIds) : IQuery<List<CheckoutItemDto>>;

}
