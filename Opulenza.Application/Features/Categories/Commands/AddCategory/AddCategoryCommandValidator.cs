using FluentValidation;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandValidator: AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x=>x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must be less than 200 characters.")
            .MustAsync(async (name, ct) => await categoryRepository.ExistsAsync(name, ct) == false)
            .WithMessage("Category already exists.");
        
        RuleFor(x=>x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

        RuleFor(x => x.ParentId)
            .GreaterThan(0).WithMessage("Invalid parent id.")
            .MustAsync(async (id, ct) => await categoryRepository.ExistsAsync(id.Value, ct)).When((x)=>x.ParentId != null)
            .WithMessage("Parent category does not exist.");
    }
}
