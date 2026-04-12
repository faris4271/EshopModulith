using Catalog.Category.Dtos;
using FluentValidation;

namespace Catalog.Features.Categorys.CreatCategory
{
    public class CreatCategoryValidator : AbstractValidator<CreatCategoryCommand>
    {
        public CreatCategoryValidator()
        {
            RuleFor(x => x.CategoryDto)
                .NotNull()
                .WithMessage("Category data is required.");

            RuleFor(x => x.CategoryDto.Slug)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CategoryDto.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CategoryDto.DisplayOrder)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryDto.ParentId)
                .Must(parentId => parentId == null || parentId != Guid.Empty)
                .WithMessage("ParentId must be a valid Guid or null.");
        }
    }
}
