using FluentValidation;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator: AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x=>x.CategoryId)
            .NotEmpty().WithMessage("Category Id is required.")
            .GreaterThan(0).WithMessage("Invalid category id.");
    }
}
