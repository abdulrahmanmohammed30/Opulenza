using FluentValidation;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;

public class DeleteCategoryImageCommandValidator: AbstractValidator<DeleteCategoryImageCommand>
{
    public DeleteCategoryImageCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category Id is required.")
            .GreaterThan(0).WithMessage("Category Id must be greater than 0.");

        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage("Image Id is required.")
            .GreaterThan(0).WithMessage("Image Id must be greater than 0.");
    }
}