using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Mapster;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.GetProductOptions
{
    internal class GetProductOptionsQueryHandler(IGenericeRepository<ProductOption,CatalogDbContext> _repository) : IQueryHandler<GetProductOptionsQuery, List<ProductOptionDto>>
    {
        public async Task<Result<List<ProductOptionDto>>> Handle(GetProductOptionsQuery request, CancellationToken cancellationToken)
        {
            var productOptions = await _repository.GetAllAsync(cancellationToken);

            var productOptionDtos = productOptions.Adapt<List<ProductOptionDto>>();



            return Result<List<ProductOptionDto>>.Success(productOptionDtos);


        }
    }
}
