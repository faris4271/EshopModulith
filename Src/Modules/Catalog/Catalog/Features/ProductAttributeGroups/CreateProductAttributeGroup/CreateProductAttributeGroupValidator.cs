using Catalog.Products.Models;
using FluentValidation;
using Shared.Contract;

namespace Catalog.Features.ProductAttributeGroups.CreateProductAttributeGroup;

public sealed class CreateProductAttributeGroupValidator : AbstractValidator<CreateProductAttributeGroupCommand>
{
    public CreateProductAttributeGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
