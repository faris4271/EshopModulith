using Catalog.Category.Dtos;
using FluentValidation;

namespace Catalog.Features.Categorys.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.categoryDto)
                .NotNull()
                .WithMessage("Category data is required.");

            RuleFor(x => x.categoryDto.Slug)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.categoryDto.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.categoryDto.DisplayOrder)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Category Id is required.");
        }
    }
}
