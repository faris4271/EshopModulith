using Catalog.Category.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Categorys.GetCategory
{
    public record GetCategoryQuery(Guid Id) : IQuery<GetCategoryDto>;
}
