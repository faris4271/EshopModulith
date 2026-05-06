using Catalog.Data;
using EShop.Module.Core.Contract.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.DeleteCategory
{
    internal class DeleteCategoryCommandHandler(IGenericeRepository<Category.Models.Category, CatalogDbContext> repository, IEntityService _entityService)
        : ICommandHandler<DeleteCategoryCommand, Result>
    {
        const string EntityType = "Category";
        public async Task<Result<Result>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            using (var begintracking = repository.BeginTransaction())
            {

                try
                {
                    var category = await repository.GetByIdAsync(request.Id);
                    if (category is null)
                        return Result.Failure<Result>(Error.NullValue);

                    await _entityService.Remove(request.Id, EntityType);

                    repository.Delete(category.Id);
                    await repository.SaveChangesAsync();

                    await begintracking.CommitAsync();

                    return Result.Success();

                   
                }
                catch (Exception ex)
                {
                    begintracking.Rollback();

                    return Result.Failure(new Error("500", ex.Message,ErrorType.Problem));
                   
                }
            }
        }
    }
}