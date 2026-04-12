using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand<Result>;
}
