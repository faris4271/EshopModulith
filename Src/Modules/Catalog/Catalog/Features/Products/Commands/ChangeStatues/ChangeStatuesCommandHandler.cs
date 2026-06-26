using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.ChangeStatues
{
    internal sealed class ChangeStatuesCommandHandler(IGenericeRepository<Product, CatalogDbContext> _productRepository) : ICommandHandler<ChangeStatuesCommand>
    {
        public async Task<Result> Handle(ChangeStatuesCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.id, cancellationToken);
            if (product == null)
            {
                return Result.Failure(new Error("404", "Product not found", ErrorType.NotFound));
            }


            product.IsPublished = !product.IsPublished;
            await _productRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
