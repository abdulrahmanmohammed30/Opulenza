using FluentValidation;

namespace Opulenza.Application.Features.ProductCategories.Commands.AddCategoriesToProduct;

public class AddCategoriesToProductCommandValidator: AbstractValidator<AddCategoriesToProductCommand>
{
        public AddCategoriesToProductCommandValidator()
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