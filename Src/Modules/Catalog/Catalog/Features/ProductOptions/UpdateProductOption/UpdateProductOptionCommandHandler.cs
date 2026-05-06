using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.UpdateProductOption
{
    internal class UpdateProductOptionCommandHandler(
        IGenericeRepository<ProductOption,CatalogDbContext> _repository) 
        : ICommandHandler<UpdateProductOptionCommand>
    {
        public async Task<Result> Handle(UpdateProductOptionCommand request, CancellationToken cancellationToken)
        {
            if ( request == null ) 
                return Result.Failure(Error.NullValue);

            var productOption = await _repository.GetByIdAsync(request.ProductOption.Id);

            if ( productOption == null ) 
                return Result.Failure(Error.NullValue);
            productOption.Name = request.ProductOption.Name;

             _repository.Update(productOption);
            return Result.Success();

        }
    }
}
