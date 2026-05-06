using Catalog.Brands.Dtos;
using Catalog.Brands.Moddels;
using Catalog.Data;
using CatalogContract.Dtos;
using Mapster;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Brands.GetBrands
{
    internal class GetBrandsQueryHandler(IGenericeRepository<Brand,CatalogDbContext> _repository) : IQueryHandler<GetBrandsQuery, List<GetBrandDto>>
    {
        public async Task<Result<List<GetBrandDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands =await _repository.GetAllAsync();

            if (!brands.Any())
                return Result.Failure<List<GetBrandDto>>(Error.NotFound("404", "can not find brands"));

            var branddto = brands.Adapt<List<GetBrandDto>>();
            if (branddto == null)
                return Result.Failure<List<GetBrandDto>>(Error.NotFound("404", "can not map brands"));
            return Result.Success(branddto);


             
        }
    }
}
