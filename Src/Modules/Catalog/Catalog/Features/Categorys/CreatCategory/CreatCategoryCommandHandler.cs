using Catalog.Category.Dtos;
using Catalog.Category.Models;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias.CreatMedia;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.DDD;

namespace Catalog.Features.Categorys.CreatCategory

{
    public record CreatCategoryCommand(CategoryDto CategoryDto) : ICommand<Guid>;


    internal class CreatCategoryCommandHandler(
        IGenericeRepository<Category.Models.Category, CatalogDbContext> _repository,
        IEntityService _entityService, ISender _sender) : ICommandHandler<CreatCategoryCommand, Guid>
    {
        const string EntityTypeId = "Category";
        public async Task<Result<Guid>> Handle(CreatCategoryCommand request, CancellationToken cancellationToken)
        {
            

            if (request.CategoryDto is null)
                return Result.Failure<Guid>(Error.NullValue);


                var category = new Category.Models.Category
                (
                 new Name(request.CategoryDto.Name),
                 request.CategoryDto.MetaTitle,
                 request.CategoryDto.MetaKeywords,
                 request.CategoryDto.MetaDescription,
                 new Description(request.CategoryDto.Description),
                 request.CategoryDto.DisplayOrder,
                 request.CategoryDto.IsPublished,
                 request.CategoryDto.ParentId
                );
                var safeSlog = await _entityService.ToSafeSlug(request.CategoryDto.Slug, category.Id, EntityTypeId);

                category.AddSafeSluge(safeSlog);

                if (request.CategoryDto.ThumbnailImages != null && 
                request.CategoryDto.ThumbnailImages.Count > 0 && request.CategoryDto.ThumbnailImages.Count<=1)
                {
                    var mediaId = await _sender.Send(new CreatMediaCommand(request.CategoryDto.ThumbnailImages));
                    if (!mediaId.IsSuccess) return Result.Failure<Guid>(mediaId.Error);

                    category.AddMediaId(new CategoryMedia(category.Id, mediaId.Value.FirstOrDefault().id,0));
                }
                

                await _repository.AddAsync(category);

               await _entityService.Add(request.CategoryDto.Name, request.CategoryDto.Slug, category.Id, EntityTypeId);

                await _repository.SaveChangesAsync();

               

                return Result.Create(category.Id);


            


        }
    }
}
