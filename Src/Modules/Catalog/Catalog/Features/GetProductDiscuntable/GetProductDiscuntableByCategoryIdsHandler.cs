using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using CatalogContract.Features.GetProductDiscuntable;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.GetProductDiscuntable
{
    internal class GetProductDiscuntableByCategoryIdsHandler(IGenericeRepository<Product, CatalogDbContext> _repository)
        : IQueryHandler<GetProductDiscuntableByCategoryIdsQuery, List<DiscountableProductDto>>
    {
        public async Task<Result<List<DiscountableProductDto>>> Handle(GetProductDiscuntableByCategoryIdsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();
            query = query.Include(x => x.Categories).ThenInclude(x => x.Category);

            var product = query.Where(x => x.Categories.Any(c => request.CategoryIds.Contains(c.CategoryId))).ToList();

            var discountedProducts = product.Select(x => new DiscountableProductDto
            {
                Id = x.Id,
                Name = x.Name.name,
                Price = x.Price.Amount
            }).ToList();

            return Result.Success(discountedProducts);
        }
    }
}
