using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.GetProductAttributes
{
    internal class GetProductAttributesQueryHandler(IGenericeRepository<ProductAttribute, CatalogDbContext> _repository) : IQueryHandler<GetProductAttributesQuery, IEnumerable<CreateProductAttributeDto>>
    {
        public async Task<Result<IEnumerable<CreateProductAttributeDto>>> Handle(GetProductAttributesQuery request, CancellationToken ct)
        {
            var attributes = await _repository.GetAllAsync(ct);

            var result = attributes.Select(a => new CreateProductAttributeDto
            {
                Id = a.Id,
                Name = a.Name.name,
                GroupId = a.GroupId
            });

            return Result.Success(result);
        }
    }
}
