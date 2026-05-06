using Catalog.Data;
using Catalog.Features.ProductOptions.CreateProductOption;
using Catalog.Products.Models;
using MediatR;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.CreatProductOption
{
    public class CreatProductOptionCommandHandler(
        IGenericeRepository<ProductOption,CatalogDbContext> _repository
        ) 
        : ICommandHandler<CreateProductOptionCommand>
    {
        public async Task<Result> Handle(CreateProductOptionCommand request, CancellationToken cancellationToken)
        {
            if ( request == null ) 
                return Result.Failure(Error.NullValue);

            var productOption = new ProductOption
            {
               Name= request.ProductOption.Name
            };

            await _repository.AddAsync(productOption, cancellationToken);
            return Result.Success();
        }
    }
}
