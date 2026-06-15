using Catalog.Category.Dtos;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using MediatR;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.GetCategory
{
    internal class GetCategoryQueryHandler(IGenericeRepository<Category.Models.Category, CatalogDbContext> repository, ISender sender)
        : IQueryHandler<GetCategoryQuery, GetCategoryDto>
    {
        public async Task<Result<GetCategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id, false, x => x.Parent);

            string thumbnailUrl = "";


            if (entity.ThumbnailImageId != Guid.Empty)
            {
                var media = await sender.Send(new GetMediaByIdQuery(entity.ThumbnailImageId));

                thumbnailUrl = media.IsSuccess ? media.Value[0].FileName : null;
            }
            var result = new GetCategoryDto
            {
                Id = request.Id,
                Name = entity.Name.name,
                Description = entity.Description.description,
                Slug = entity.Slug,
                ThumbnailImageUrl = thumbnailUrl,
                DisplayOrder = entity.DisplayOrder,
                IncludeInMenu = entity.IncludeInMenu,
                IsPublished = entity.IsPublished,
                MetaDescription = entity.MetaDescription,
                MetaKeywords = entity.MetaKeywords,
                MetaTitle = entity.MetaTitle,
                ParentId = entity.ParentId,
            };



            while (entity.Parent != null)
            {
                result.Name = $"{entity.Parent.Name.name} >> {result.Name}";
            }

            return Result.Success(result);

        }


    }
}
