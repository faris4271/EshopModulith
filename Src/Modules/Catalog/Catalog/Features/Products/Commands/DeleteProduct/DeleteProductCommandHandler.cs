using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.DeleteProduct
{
    internal class DeleteProductCommandHandler(IGenericeRepository<Product, CatalogDbContext> repository)
        : ICommandHandler<DeleteProductCommand, Result>
    {
        public async Task<Result<Result>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.Id);
            if (product is null)
                return Result.Failure<Result>(Error.NullValue);

            repository.Delete<Guid>(product.Id);
            await repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
