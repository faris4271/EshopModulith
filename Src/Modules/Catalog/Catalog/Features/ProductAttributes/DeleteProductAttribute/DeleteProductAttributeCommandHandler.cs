using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.DeleteProductAttribute
{
    internal class DeleteProductAttributeCommandHandler(IGenericeRepository<ProductAttribute, CatalogDbContext> _repository) : ICommandHandler<DeleteProductAttributeCommand>
    {
        public async Task<Result> Handle(DeleteProductAttributeCommand request, CancellationToken ct)
        {
            var attribute = await _repository.GetByIdAsync(request.Id);
            if (attribute == null)
            {
                return Result.Failure(Error.NotFound("401","Product attribute not found"));
            }

             _repository.Delete(attribute);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
