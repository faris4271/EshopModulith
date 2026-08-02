using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace CatalogContract.Features.GetCheckoutItemWithTax
{
    public record GetCheckoutItemWithTaxQuery(List<Guid> productIds) : IQuery<List<CheckoutItemWithTaxDto>>;

}
