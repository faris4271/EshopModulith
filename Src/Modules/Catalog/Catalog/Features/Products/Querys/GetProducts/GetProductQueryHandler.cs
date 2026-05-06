using Catalog.Data;
using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Catalog.Features.Products.Querys.GetProducts
{
    public record GetProductQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<Product>>;
    
    internal record GetProductQueryResponse();
    internal class GetProductQueryHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetProductQuery, SmartTableResult<Product>>
    {
        public async Task<Result<SmartTableResult<Product>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            query=query.Include(x=>x.AttributeValues);

            var prducts =  query.ToSmartTableResult(request.SmartTableParam);


            return prducts;
        }
    }
}
