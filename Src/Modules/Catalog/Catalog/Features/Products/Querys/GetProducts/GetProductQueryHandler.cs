using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Paginations;

namespace Catalog.Features.Products.Querys.GetProducts
{
    internal class GetProductQuery : IQuery<PaginatedResult<Product>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    internal record GetProductQueryResponse();
    internal class GetProductQueryHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetProductQuery, PaginatedResult<Product>>
    {
        public async Task<Result<PaginatedResult<Product>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            var prducts = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);


            return prducts;
        }
    }
}
