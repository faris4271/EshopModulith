using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.GetCategory
{
    public record GetCategoryQuery(Guid Id) : IQuery<Category.Models.Category>;
}
