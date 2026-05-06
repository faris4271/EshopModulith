using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.GetProduct
{
    internal class GetProductOptionQueryHandler(IGenericeRepository<ProductOption,CatalogDbContext> _repository) : IQueryHandler<GetProductOptionQuery, ProductOptionDto>
    {
        public async Task<Result<ProductOptionDto>> Handle(GetProductOptionQuery request, CancellationToken cancellationToken)
        {
            if(request == null)
                return Result.Failure<ProductOptionDto>(Error.NullValue);

            var option = await _repository.GetByIdAsync(request.Id);
            if(option == null)
                return Result.Failure<ProductOptionDto>(
                    Error.NotFound("ProductOptionNotFound", $"Product option with id {request.Id} was not found."));

            var productOptionDto = new ProductOptionDto
            {
                Id = option.Id,
                Name = option.Name,
            };

            return  Result.Success(productOptionDto);
        }
    }
}
