using Catalog.Data;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.GetCategory
{
    internal class GetCategoryQueryHandler(IGenericeRepository<Category.Models.Category, CatalogDbContext> repository)
        : IQueryHandler<GetCategoryQuery, Category.Models.Category>
    {
        public async Task<Result<Category.Models.Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            return entity is not null
                ? Result.Create(entity)
                : Result.Failure<Category.Models.Category>(Error.NullValue);
        }
    }
}
