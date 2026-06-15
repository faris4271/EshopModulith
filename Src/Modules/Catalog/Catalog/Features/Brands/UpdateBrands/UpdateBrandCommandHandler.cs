using Catalog.Brands.Moddels;
using Catalog.Data;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Brands.UpdateBrands
{
    internal class UpdateBrandCommandHandler(IGenericeRepository<Brand, CatalogDbContext> _repository) : ICommandHandler<UpdateBrandCommand>
    {
        public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _repository.GetByIdAsync(request.id);

            if (brand is null)
            {
                return Result.Failure(new Error("400", $"can not find this brand id{request.id}", ErrorType.Failure));
            }

            brand.Update(
                request.CreatBrandDto.Name,
                request.CreatBrandDto.Slug,
                request.CreatBrandDto.Description,
                request.CreatBrandDto.IsPublished,
                request.CreatBrandDto.IsDeleted



                );

            _repository.Update(brand);

            await _repository.SaveChangesAsync();
            return Result.Success();

        }
    }
}
