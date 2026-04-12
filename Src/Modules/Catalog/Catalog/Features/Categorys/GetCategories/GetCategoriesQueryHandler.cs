using Catalog.Data;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Paginations;

namespace Catalog.Features.Categorys.GetCategories
{
    internal class GetCategoriesQueryHandler(IGenericeRepository<Category.Models.Category, CatalogDbContext> repository)
        : IQueryHandler<GetCategoriesQuery, PaginatedResult<Category.Models.Category>>
    {
        public async Task<Result<PaginatedResult<Category.Models.Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query =await repository.GetAllAsQuerable();

            // Apply ordering or filtering if needed
            query = query.OrderBy(c => c.DisplayOrder);

            var paged = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize,cancellationToken);
            return Result.Create(paged);
        }
    }
}
