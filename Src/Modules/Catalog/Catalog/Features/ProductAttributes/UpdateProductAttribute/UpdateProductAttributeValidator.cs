using FluentValidation;
using CatalogContract.Dtos;

namespace Catalog.Features.ProductAttributes.UpdateProductAttribute
{
    public class UpdateProductAttributeValidator : AbstractValidator<UpdateProductAttributeCommand>
    {
        public UpdateProductAttributeValidator()
        {
            RuleFor(x => x.productAttributeDto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.productAttributeDto.GroupId).NotEmpty();
        }
    }
}
