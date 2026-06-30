using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.GetProductTemplets
{
    internal class GetProductTempletsQueryHandler(IGenericeRepository<ProductTemplate, CatalogDbContext> _repository) : IQueryHandler<GetProductTempletsQuery, List<GetProductTempletsDto>>
    {
        public async Task<Result<List<GetProductTempletsDto>>> Handle(GetProductTempletsQuery request, CancellationToken cancellationToken)
        {
            var productTemplates = await _repository.Query();
            var dtos = productTemplates.Select(x => new GetProductTempletsDto
            {
                Id = x.Id,
                Name = x.Name.name
            }).ToList();
            return Result.Success(dtos);
        }
    }
}
