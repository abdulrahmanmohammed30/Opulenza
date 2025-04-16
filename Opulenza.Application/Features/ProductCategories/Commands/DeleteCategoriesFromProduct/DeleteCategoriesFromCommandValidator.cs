using FluentValidation;

namespace Opulenza.Application.Features.ProductCategories.Commands.DeleteCategoriesFromProduct;

public class DeleteCategoriesFromCommandValidator: AbstractValidator<DeleteCategoriesFromProductCommand>
{
    public DeleteCategoriesFromCommandValidator()
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