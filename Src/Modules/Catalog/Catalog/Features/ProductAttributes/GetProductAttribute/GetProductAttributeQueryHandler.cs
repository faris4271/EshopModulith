using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    internal class GetProductAttributeQueryHandler(IGenericeRepository<ProductAttribute, CatalogDbContext> _repository)
        : IQueryHandler<GetProductAttributeQuery, ProductAttributeDto>
    {
        public async Task<Result<ProductAttributeDto>> Handle(GetProductAttributeQuery request, CancellationToken ct)
        {
            var attribute = await _repository.GetByIdAsync(request.Id);
            if (attribute == null)
            {
                return Result.Failure<ProductAttributeDto>(Error.NotFound("401","Product attribute not found"));
            }
            
            var productAttributeDto = attribute.Adapt<ProductAttributeDto>();
            return Result.Success(productAttributeDto);
        }
    }
}
