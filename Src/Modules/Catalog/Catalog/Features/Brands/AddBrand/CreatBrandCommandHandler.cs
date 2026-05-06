using Catalog.Brands.Moddels;
using Catalog.Data;
using EShop.Module.Core.Contract.Services;
using Mapster;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Brands.AddBrand
{
    internal class CreatBrandCommandHandler(
        IGenericeRepository<Brand,CatalogDbContext> _repository,
        IEntityService _entityService) : ICommandHandler<CreatBrandCommand, Guid>
    {
        const string EntityTypeId = "Brand";
        public async Task<Result<Guid>> Handle(CreatBrandCommand request, CancellationToken cancellationToken)
        {
            if (request.creatBrandDto == null)
                return Result.Failure<Guid>(Error.NotFound("404", "brand is null"));

            var brand = new Brand(request.creatBrandDto.Name
                , request.creatBrandDto.Description, request
                .creatBrandDto.IsPublished, request.creatBrandDto.IsDeleted);

            var safeSloge =await _entityService.ToSafeSlug(request.creatBrandDto.Slug, brand.Id, EntityTypeId);

            brand.AddSafeSloge(safeSloge);

          await  _entityService.Add(brand.Name,safeSloge,brand.Id, EntityTypeId);


           await _repository.AddAsync(brand);

        await _repository.SaveChangesAsync();

           return Result.Create(Guid.NewGuid());
        }
    }
}
