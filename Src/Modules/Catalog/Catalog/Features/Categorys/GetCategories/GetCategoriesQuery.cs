using Shared.Contract.CQRS;
using Shared.Paginations;

namespace Catalog.Features.Categorys.GetCategories
{
    public record GetCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedResult<Category.Models.Category>>;
}
