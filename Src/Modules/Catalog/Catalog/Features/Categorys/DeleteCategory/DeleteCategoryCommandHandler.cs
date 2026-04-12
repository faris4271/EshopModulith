using Catalog.Data;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.DeleteCategory
{
    internal class DeleteCategoryCommandHandler(IGenericeRepository<Category.Models.Category, CatalogDbContext> repository)
        : ICommandHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result<Result>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await repository.GetByIdAsync(request.Id);
            if (category is null)
                return Result.Failure<Result>(Error.NullValue);

            repository.Delete(category.Id);
            await repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
