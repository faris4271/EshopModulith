using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.CreatProductTemplet
{
    internal class CreatProductTempletCommandHandler(IGenericeRepository<ProductTemplate, CatalogDbContext> _productTemplateRepository) : ICommandHandler<CreatProductTempletCommand>
    {
        public async Task<Result> Handle(CreatProductTempletCommand request, CancellationToken cancellationToken)
        {
            if (request.TempletDto is null)
                return Result.Failure(Error.NullValue);

            var productTemplate = ProductTemplate.Create(request.TempletDto.Name);

            foreach (var attributeId in request.TempletDto.AttributeIds)
            {
                productTemplate.AddAttribute(attributeId);
            }

            await _productTemplateRepository.AddAsync(productTemplate);
            await _productTemplateRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
