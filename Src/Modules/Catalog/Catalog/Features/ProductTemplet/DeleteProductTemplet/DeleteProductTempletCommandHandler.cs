using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.DeleteProductTemplet
{
    internal class DeleteProductTempletCommandHandler(IGenericeRepository<ProductTemplate, CatalogDbContext> _productTemplateRepository) : ICommandHandler<DeleteProductTempletCommand>
    {
        public async Task<Result> Handle(DeleteProductTempletCommand request, CancellationToken cancellationToken)
        {
            var productTemplate = await _productTemplateRepository.GetByIdAsync(request.Id);

            if (productTemplate is null)
                return Result.Failure(Error.NullValue);

            _productTemplateRepository.Delete(productTemplate);
            await _productTemplateRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}