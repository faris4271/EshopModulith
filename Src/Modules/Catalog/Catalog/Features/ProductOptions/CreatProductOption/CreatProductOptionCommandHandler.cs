using Catalog.Data;
using Catalog.Features.ProductOptions.CreateProductOption;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductOptions.CreatProductOption
{
    public class CreatProductOptionCommandHandler(
        IGenericeRepository<ProductOption, CatalogDbContext> _repository
        )
        : ICommandHandler<CreateProductOptionCommand>
    {
        public async Task<Result> Handle(CreateProductOptionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.Failure(Error.NullValue);

            var productOption = new ProductOption(request.ProductOption.Name);

            //productOption.AddValues(new ProductOptionValue())


            await _repository.AddAsync(productOption, cancellationToken);
            await _repository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
