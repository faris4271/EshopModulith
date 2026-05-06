using FluentValidation;
using Shared.Contract;

namespace Catalog.Features.ProductAttributeGroups.UpdateProductAttributeGroup;

public sealed class UpdateProductAttributeGroupValidator : AbstractValidator<UpdateProductAttributeGroupCommand>
{
    public UpdateProductAttributeGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
