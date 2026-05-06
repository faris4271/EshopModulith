using Catalog.Category.Dtos;
using Catalog.Data;
using Catalog.Features.Categorys.GetCategory;
using EShop.Module.Core.Contract.Feature.Medias;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Categorys.GetCategories
{
    internal class GetCategoriesQueryHandler(
        IGenericeRepository<Category.Models.Category, 
          CatalogDbContext> repository,ISender sender)
        : IQueryHandler<GetCategoriesQuery,List<GetCategoryDto>>
    {
        public async Task<Result<List<GetCategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query =await repository.GetAllAsQuerable();

            // Apply ordering or filtering if needed
            query = query.OrderBy(c => c.DisplayOrder).Include(x=>x.ThumbnailImage);

           var categories = await query.ToListAsync();
            var categoryDtos = new List<GetCategoryDto>();

            // Only take media ids from categories that actually have a ThumbnailImage
            var mediaIds = categories
                .Where(c => c.ThumbnailImage != null)
                .Select(c => c.ThumbnailImage!.MediaId)
                .ToList();

            var medias = await sender.Send(new GetListOfMediasQuery(mediaIds));

            if ( medias.Value.Any())
            {
                categoryDtos = categories.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name.name,
                    Description = x.Description.description,
                    DisplayOrder = x.DisplayOrder,
                    MetaDescription = x.MetaDescription,
                    IncludeInMenu = x.IncludeInMenu,
                    Slug = x.Slug,
                    ParentId = x.ParentId,
                    IsPublished = x.IsPublished,
                    MetaKeywords = x.MetaKeywords,
                    MetaTitle = x.MetaTitle,
                    ThumbnailImageUrl = medias.Value.FirstOrDefault(m => m.id == x.ThumbnailImage?.MediaId)?.FileName
                }).ToList();
            }
            else
            {
             
                categoryDtos = categories.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name.name,
                    Description = x.Description.description,
                    DisplayOrder = x.DisplayOrder,
                    MetaDescription = x.MetaDescription,
                    IncludeInMenu = x.IncludeInMenu,
                    Slug = x.Slug,
                    ParentId = x.ParentId,
                    IsPublished = x.IsPublished,
                    MetaKeywords = x.MetaKeywords,
                    MetaTitle = x.MetaTitle,
                    ThumbnailImageUrl = null
                }).ToList();
            }

            return Result.Success<List<GetCategoryDto>>(categoryDtos);
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
