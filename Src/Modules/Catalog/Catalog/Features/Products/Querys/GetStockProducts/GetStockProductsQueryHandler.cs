using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using CatalogContract.Features.GetStockProducts;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Querys.GetStockProducts
{
    internal sealed class GetStockProductsQueryHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetStockProductsQuery, List<GetProductStockDto>>
    {
        public async Task<Result<List<GetProductStockDto>>> Handle(GetStockProductsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();
            var products = query.Where(p => request.ProductId.Contains(p.Id)).AsNoTracking()
                .Select(p => new GetProductStockDto(p.Id, p.Name.name, p.Sku)).ToList();
            if (products == null)
            {
                return Result.Failure<List<GetProductStockDto>>(Error.NotFound("404", "Product not found"));
            }


            return Result.Success(products);
        }
    }
}
