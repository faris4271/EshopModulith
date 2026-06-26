using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.UpdateProductTemplet
{
    internal class UpdateProductTempletCommandHandler(
        IGenericeRepository<ProductTemplate, CatalogDbContext> _productTemplateRepository,
        IGenericeRepository<ProductTemplateProductAttribute, CatalogDbContext> _productTemplateProductAttributeRepository
        )
        : ICommandHandler<UpdateProductTempletCommand>
    {
        public async Task<Result> Handle(UpdateProductTempletCommand request, CancellationToken cancellationToken)
        {
            if (request.TempletDto is null)
                return Result.Failure(Error.NullValue);

            var productTemplate = await _productTemplateRepository.GetByIdAsync(request.TempletDto.Id, true, x => x.ProductAttributes);

            if (productTemplate is null)
                return Result.Failure(Error.Failure("400", "Product template not found."));


            foreach (var attributeId in request.TempletDto.AttributeIds)
            {
                if (!productTemplate.ProductAttributes.Any(x => x.ProductAttributeId == attributeId))
                {
                    productTemplate.AddAttribute(attributeId);
                }

            }

            var deletedAttributes = productTemplate.ProductAttributes.Where(attr => !request.TempletDto.AttributeIds.Contains(attr.ProductAttributeId));

            foreach (var attribute in deletedAttributes.ToList())
            {
                _productTemplateProductAttributeRepository.Delete(attribute);
            }


            await _productTemplateRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}