using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    internal class GetProductAttributeByIdQueryHandler(IGenericeRepository<ProductAttribute, CatalogDbContext> _repository)
        : IQueryHandler<GetProductAttributeByIdQuery, ProductAttributeDto>
    {
        public async Task<Result<ProductAttributeDto>> Handle(GetProductAttributeByIdQuery request, CancellationToken ct)
        {
            var attribute = await _repository.GetByIdAsync(request.Id, false, x => x.Group);
            if (attribute == null)
            {
                return Result.Failure<ProductAttributeDto>(Error.NotFound("401", "Product attribute not found"));
            }

            var productAttributeDto = new ProductAttributeDto
            {
                Id = attribute.Id,
                Name = attribute.Name.name,
                GroupName = attribute.Group.Name.name
            };
            return Result.Success(productAttributeDto);
        }
    }
}
