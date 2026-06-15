using Catalog.Category.Dtos;
using Catalog.Data;
using EShop.Module.Core.Contract.Feature.Medias.UpdateMedia;
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
        const string EntityTypeId = "Category";
        public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAllAsQuerable();

            var categiry = query.FirstOrDefault(x => x.Id == request.id);

            if (categiry == null) return Result.Failure<Guid>(Error.NullValue);

            if (categiry.ParentId.HasValue)
            {
                var isCircular = await HaveCircularNesting(categiry.Id, categiry.ParentId);

                if (!isCircular) return Result.Failure<Guid>(Error.NullValue);
            }

            if (categiry.ThumbnailImageId != Guid.Empty)
            {
                var listIds = new List<Guid>();
                listIds.Add(categiry.ThumbnailImageId);

                var mediaId = await sender.Send(new UpdateMediaCommand(listIds, request.categoryDto.ThumbnailImages));

                if (!mediaId.IsSuccess)
                    return Result.Failure<Guid>(mediaId.Error);
            }



            var safeSlog = await _entityService.ToSafeSlug(request.categoryDto.Slug, categiry.Id, EntityTypeId);

            categiry.AddSafeSluge(safeSlog);

            await _entityService.Update(categiry.Name.name, safeSlog, categiry.Id, EntityTypeId);

            categiry.Update(request.categoryDto.Name
               , request.categoryDto.MetaTitle,
                 safeSlog,
                 request.categoryDto.MetaKeywords, request.categoryDto.MetaDescription
                , request.categoryDto.Description, request.categoryDto.DisplayOrder,
                request.categoryDto.IsPublished, request.categoryDto.ParentId);

            _repository.Update(categiry);

            await _repository.SaveChangesAsync();

            return Result.Success<Guid>(categiry.Id);

        }

        private async Task<bool> HaveCircularNesting(Guid childId, Guid? parentId)
        {

            var sql = @"
        WITH RECURSIVE CategoryTree AS (
            SELECT ""Id"", ""ParentId"" FROM ""catalog"".""Categories"" WHERE ""Id"" = {0}
            UNION ALL
            SELECT c.""Id"", c.""ParentId"" FROM ""catalog"".""Categories"" c
            INNER JOIN CategoryTree ct ON c.""Id"" = ct.""ParentId""
        )
        SELECT EXISTS (SELECT 1 FROM CategoryTree WHERE ""Id"" = {1})";

            var isCircular = await _context.Database
                .SqlQueryRaw<bool>(sql, parentId, childId)
                .FirstOrDefaultAsync();

            return isCircular;
        }
    }
}
