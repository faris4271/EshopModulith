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
    internal class GetProductDiscuntableHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetProductDiscuntableQuery, List<DiscountableProductDto>>
    {
        public async Task<Result<List<DiscountableProductDto>>> Handle(GetProductDiscuntableQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var products = await query.Where(p => request.ProductIds.Contains(p.Id)).ToListAsync(cancellationToken);

            // Map products to discounted product DTOs
            var discountedProducts = products.Select(p => new DiscountableProductDto
            {
                Id = p.Id,
                Name = p.Name.name,
                Price = p.Price.Amount,
            }).ToList();

            return Result.Success(discountedProducts);
        }
    }
}
