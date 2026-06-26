using Catalog.Brands.Moddels;
using Catalog.Data;
using CatalogContract.Dtos;
using Mapster;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Brands.GetBrands
{
    internal class GetBrandsQueryHandler(IGenericeRepository<Brand, CatalogDbContext> _repository) : IQueryHandler<GetBrandsQuery, List<GetBrandDto>>
    {
        public async Task<Result<List<GetBrandDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _repository.GetAllAsync();

            var branddto = brands.Adapt<List<GetBrandDto>>();

            return Result.Success(branddto);



        }
    }
}
