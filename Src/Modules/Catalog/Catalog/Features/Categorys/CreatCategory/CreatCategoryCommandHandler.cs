using Catalog.Category.Dtos;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias;
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
        const string EntityTypeId = "Categort";
        public async Task<Result<Guid>> Handle(CreatCategoryCommand request, CancellationToken cancellationToken)
        {

            if (request.CategoryDto is null)
                return Result.Failure<Guid>(Error.NullValue);

            using (var transaction = _repository.BeginTransaction())
            {

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

                var mediaId = await _sender.Send(new CreatMediaCommand(request.CategoryDto.ThumbnailImage));
                if (!mediaId.IsSuccess) return Result.Failure<Guid>(mediaId.Error);

                category.AddMediaId(mediaId.Value.id);

                _repository.Add(category);

                _entityService.Add(request.CategoryDto.Name, request.CategoryDto.Slug, request.CategoryDto.Id, EntityTypeId);

                await _repository.SaveChangesAsync();

                transaction.Commit();

                return Result.Create(category.Id);


            }


        }
    }
}
