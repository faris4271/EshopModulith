using FluentValidation;
using CatalogContract.Dtos;

namespace Catalog.Features.ProductAttributes.CreateProductAttribute
{
    public class CreateProductAttributeValidator : AbstractValidator<CreateProductAttributeCommand>
    {
        public CreateProductAttributeValidator()
        {
            RuleFor(x => x.productAttributeDto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.productAttributeDto.GroupId).NotEmpty()
                 .WithMessage("GroupId is required.");
             
        }
    }
}
