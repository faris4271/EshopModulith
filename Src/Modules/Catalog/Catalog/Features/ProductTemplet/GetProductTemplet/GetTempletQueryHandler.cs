using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.GetProductTemplet
{
    internal class GetTempletQueryHandler(
        IGenericeRepository<ProductTemplate, CatalogDbContext> _productTemplateRepository)
        : IQueryHandler<GetTempletQuery, ProductTempletDto>
    {
        public async Task<Result<ProductTempletDto>> Handle(GetTempletQuery request, CancellationToken cancellationToken)
        {

            var query = await _productTemplateRepository.GetAllAsQuerable();
            var productTemplate = query.Include(x => x.ProductAttributes)
             .ThenInclude(x => x.ProductAttribute).ThenInclude(x => x.Group)
             .FirstOrDefault(x => x.Id == request.id);

            var model = new ProductTempletDto
            {
                Id = productTemplate.Id,
                Name = productTemplate.Name.name,
                Attributes = productTemplate.ProductAttributes.Select(
                    x => new ProductAttributeDto()
                    {
                        Id = x.ProductAttributeId,
                        Name = x.ProductAttribute.Name.name,
                        GroupName = x.ProductAttribute.Group.Name.name
                    }).ToList()
            };

            return Result.Success(model);
        }
    }
}
