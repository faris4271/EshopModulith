using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.DeletProductOption
{
    internal class DeletProductOptionCommandHandler(IGenericeRepository<ProductOption,CatalogDbContext> _repository) : ICommandHandler<DeletProductOptionCommand>
    {
        public async Task<Result> Handle(DeletProductOptionCommand request, CancellationToken cancellationToken)
        {
           if (request == null)
               return Result.Failure(Error.NullValue);

           var productOption = await _repository.GetByIdAsync(request.id);

           if (productOption == null)
               return Result.Failure(Error.NullValue);

           _repository.Delete(productOption);
           return Result.Success();
        }
    }
}
