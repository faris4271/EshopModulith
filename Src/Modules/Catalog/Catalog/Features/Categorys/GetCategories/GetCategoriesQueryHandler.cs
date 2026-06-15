using Catalog.Category.Dtos;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Categorys.GetCategories
{
    internal class GetCategoriesQueryHandler(
        IGenericeRepository<Category.Models.Category,
          CatalogDbContext> repository, ISender sender)
        : IQueryHandler<GetCategoriesQuery, List<GetCategoryDto>>
    {
        public async Task<Result<List<GetCategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = await repository.GetAllAsQuerable();

            // Apply ordering or filtering if needed
            query = query.Include(x => x.Parent).OrderBy(c => c.DisplayOrder);

            var categories = await query.ToListAsync();
            var categoryDtos = new List<GetCategoryDto>();

            // Only take media ids from categories that actually have a ThumbnailImage
            var mediaIds = categories
                .Where(c => c.ThumbnailImageId != null)
                .Select(c => c.ThumbnailImageId)
                .ToList();

            var medias = await sender.Send(new GetListOfMediasQuery(mediaIds));

            foreach (var category in categories)
            {
                var categoryDto = new GetCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name.name,
                    Description = category.Description.description,
                    DisplayOrder = category.DisplayOrder,
                    MetaDescription = category.MetaDescription,
                    IncludeInMenu = category.IncludeInMenu,
                    Slug = category.Slug,
                    ParentId = category.ParentId,
                    IsPublished = category.IsPublished,
                    MetaKeywords = category.MetaKeywords,
                    MetaTitle = category.MetaTitle,
                    ThumbnailImageUrl = medias.Value.FirstOrDefault(m => m.id == category.ThumbnailImageId)?.FileName ?? string.Empty,
                };

                var parentCategory = category.Parent;

                while (parentCategory != null)
                {
                    categoryDto.Name = $"{parentCategory.Name.name} >> {categoryDto.Name}";
                    parentCategory = parentCategory.Parent;
                }

                categoryDtos.Add(categoryDto);


            }


            return Result.Success(categoryDtos);
        }


        //private void MapMediaUrl(Category.Models.Category category ,GetCategoryDto categoryDto, string mediaUrl)
        //{
        //   categoryDto.ThumbnailImageUrl=mediaUrl;
        //    categoryDto.Id=category.Id;
        //    categoryDto.Name=category.Name.name;
        //    categoryDto.Description=category.Description.description;
        //    categoryDto.MetaTitle=category.MetaTitle;
        //    categoryDto.MetaKeywords=category.MetaKeywords;
        //    categoryDto.MetaDescription=category.MetaDescription;
        //    categoryDto.DisplayOrder=category.DisplayOrder;
        //    categoryDto.ParentId=category.ParentId;
        //    categoryDto.IncludeInMenu=category.IncludeInMenu;
        //    categoryDto.IsPublished=category.IsPublished;
        //    categoryDto.Slug=category.Slug;
        //    categoryDto.ThumbnailImageUrl=mediaUrl; 

        //}
    }
}
