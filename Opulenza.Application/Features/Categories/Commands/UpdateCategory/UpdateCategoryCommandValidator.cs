using FluentValidation;

namespace Opulenza.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator: AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty().WithMessage("Category Id is required.")
            .GreaterThan(0).WithMessage("Invalid category id.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must be less than 200 characters.");
        
        RuleFor(x=>x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

        RuleFor(x => x.ParentId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Invalid parent id.");
    }
}