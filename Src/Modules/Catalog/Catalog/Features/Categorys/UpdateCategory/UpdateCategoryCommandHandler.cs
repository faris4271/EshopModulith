using Catalog.Category.Dtos;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.UpdateCategory
{
    public record UpdateCategoryCommand(CategoryDto categoryDto, Guid id) : ICommand<Guid>;
    internal class UpdateCategoryCommandHandler(
        IGenericeRepository<Category.Models.Category,
            CatalogDbContext> _repository,
        CatalogDbContext _context, ISender sender,
        IEntityService _entityService)
        : ICommandHandler<UpdateCategoryCommand, Guid>
    {
        const string EntityTypeId = "Categort";
        public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAllAsQuerable();

            var categiry = query.FirstOrDefault(x => x.Id == request.id);

            if (categiry == null) return Result.Failure<Guid>(Error.NullValue);

            var isCircular = await HaveCircularNesting(request.categoryDto.Id.ToString(), request.categoryDto.ParentId.ToString());

            if (!isCircular) return Result.Failure<Guid>(Error.NullValue);

            var mediaId = await sender.Send(new CreatMediaCommand(request.categoryDto.ThumbnailImage));

            if (!mediaId.IsSuccess)
                return Result.Failure<Guid>(mediaId.Error);

            var safeSlog = await _entityService.ToSafeSlug(request.categoryDto.Slug, request.categoryDto.Id, EntityTypeId);

            categiry.Update(request.categoryDto.Name
                , request.categoryDto.MetaTitle,
                safeSlog,
                request.categoryDto.MetaKeywords, request.categoryDto.MetaDescription
                , request.categoryDto.Description, request.categoryDto.DisplayOrder,
                request.categoryDto.IsPublished, request.categoryDto.ParentId);

            _repository.Add(categiry);

            await _repository.SaveChangesAsync();

            return Result.Success<Guid>(categiry.Id);



        }

        private async Task<bool> HaveCircularNesting(string childId, string parentId)
        {
            var sql = @"
        WITH CategoryTree AS (
            SELECT Id, ParentId FROM Categories WHERE Id = {0}
            UNION ALL
            SELECT c.Id, c.ParentId FROM Categories c
            INNER JOIN CategoryTree ct ON c.Id = ct.ParentId
        )
        SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM CategoryTree WHERE Id = {1}) THEN 1 ELSE 0 END AS BIT)";


            var isCircular = await _context.Database
                .SqlQueryRaw<bool>(sql, parentId, childId)
                .FirstOrDefaultAsync();

            return isCircular;
        }
    }
}
