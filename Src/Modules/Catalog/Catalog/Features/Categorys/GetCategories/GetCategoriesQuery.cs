using Catalog.Category.Dtos;
using Shared.Contract.CQRS;


namespace Catalog.Features.Categorys.GetCategories
{
    public record GetCategoriesQuery() : IQuery<List<GetCategoryDto>>;
}
