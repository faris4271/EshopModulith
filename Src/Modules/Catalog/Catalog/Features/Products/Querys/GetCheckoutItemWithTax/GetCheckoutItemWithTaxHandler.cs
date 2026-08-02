using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using CatalogContract.Features.GetCheckoutItemWithTax;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Querys.GetCheckoutItemWithTax
{
    internal class GetCheckoutItemWithTaxHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetCheckoutItemWithTaxQuery, List<CheckoutItemWithTaxDto>>
    {
        public async Task<Result<List<CheckoutItemWithTaxDto>>> Handle(GetCheckoutItemWithTaxQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var result = query.Where(p => request.productIds.Contains(p.Id))
                              .Select(p => new CheckoutItemWithTaxDto
                              {
                                  productId = p.Id,
                                  productPrice = p.Price.Amount,
                                  taxClassId = p.TaxClassId
                              }).ToList();

            return Result.Success(result);
        }
    }
}
