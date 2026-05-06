using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Querys.GetCartRuleProduct
{
    internal class GetCartRuleProductQueryHandler(IGenericeRepository<Product,CatalogDbContext> _repository) : IQueryHandler<GetCartRuleProductQuery, List<GetCartRuleProductDto>>
    {
        public async Task<Result<List<GetCartRuleProductDto>>> Handle(GetCartRuleProductQuery request, CancellationToken cancellationToken)
        {
            var query =await _repository.GetAllAsQuerable();

            var products= query.Where(p=>request.productIds.Contains(p.Id)).Select(p => new GetCartRuleProductDto
            {
                Id = p.Id,
                Name = p.Name.name,
                IsPublished = p.IsPublished
            }).ToList();

            return Result.Success(products);
        }
    }
}
