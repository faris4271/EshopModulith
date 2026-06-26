using Catalog.Data;
using Catalog.Products.Models;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Constants;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.DeleteProduct
{
    internal class DeleteProductCommandHandler(IGenericeRepository<Product, CatalogDbContext> repository, IEntityService _entityService)
        : ICommandHandler<DeleteProductCommand, Result>
    {
        public async Task<Result<Result>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.Id);
            if (product is null)
                return Result.Failure<Result>(Error.NullValue);

            repository.Delete<Guid>(product.Id);
            await _entityService.Remove(product.Id, EntityTypeConstants.Product);
            await repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
