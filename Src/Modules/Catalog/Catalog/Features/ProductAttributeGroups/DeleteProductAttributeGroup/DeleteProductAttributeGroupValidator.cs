using FluentValidation;
using Shared.Contract;

namespace Catalog.Features.ProductAttributeGroups.DeleteProductAttributeGroup;

public sealed class DeleteProductAttributeGroupValidator : AbstractValidator<DeleteProductAttributeGroupCommand>
{
    public DeleteProductAttributeGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
