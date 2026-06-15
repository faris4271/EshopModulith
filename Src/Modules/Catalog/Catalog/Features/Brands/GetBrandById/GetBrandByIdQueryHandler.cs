using Catalog.Brands.Moddels;
using Catalog.Data;
using CatalogContract.Dtos;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Brands.GetBrandById
{
    internal class GetBrandByIdQueryHandler(IGenericeRepository<Brand, CatalogDbContext> _repository) : IQueryHandler<GetBrandByIdQuery, GetBrandDto>
    {
        public async Task<Result<GetBrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var brand = await query.FirstOrDefaultAsync(x => x.Id == request.id);

            var result = new GetBrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Slug = brand.Slug,
                IsDeleted = brand.IsDeleted,
                IsPublished = brand.IsPublished,
            };

            return Result.Success(result);

        }
    }
}
