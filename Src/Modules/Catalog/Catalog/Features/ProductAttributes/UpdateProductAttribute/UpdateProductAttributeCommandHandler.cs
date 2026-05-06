using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.UpdateProductAttribute
{
    internal class UpdateProductAttributeCommandHandler(IGenericeRepository<ProductAttribute, CatalogDbContext> _repository) : ICommandHandler<UpdateProductAttributeCommand>
    {
        public async Task<Result> Handle(UpdateProductAttributeCommand request, CancellationToken ct)
        {
            var attribute = await _repository.GetByIdAsync(request.Id, ct);
            if (attribute == null)
            {
                return Result.Failure(Error.NotFound("404", "Product attribute not found."));
            }

            attribute.Name = new Shared.DDD.Name(request.productAttributeDto.Name);
            attribute.GroupId = request.productAttributeDto.GroupId;

            _repository.Update(attribute);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
