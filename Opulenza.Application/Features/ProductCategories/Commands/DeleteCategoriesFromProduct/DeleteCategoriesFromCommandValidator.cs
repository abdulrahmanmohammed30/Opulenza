using FluentValidation;
using Opulenza.Application.Features.ProductCategories.Commands.DeleteCategoriesFromProduct;

namespace Opulenza.Application.Features.ProductCategories.Commands.DeleteCategories;

public class DeleteCategoriesCommandValidator: AbstractValidator<DeleteCategoriesFromProductCommand>
{
    public DeleteCategoriesCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .GreaterThan(0).WithMessage("Invalid product id.");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("At least one category is required.");
                
        RuleForEach(x=>x.Categories)
            .GreaterThan(0).WithMessage("Invalid category id.");
    }
}