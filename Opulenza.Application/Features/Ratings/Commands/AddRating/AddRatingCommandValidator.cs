using FluentValidation;

namespace Opulenza.Application.Features.Ratings.Commands.AddRating;

public class AddRatingCommandValidator: AbstractValidator<AddRatingCommand>
{
    public AddRatingCommandValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Value must be greater than or equal to 1.")
            .LessThanOrEqualTo(5).WithMessage("Value must be less than or equal to 5.");
        
        RuleFor(x=>x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .GreaterThan(0).WithMessage("Invalid product id.");
    }
}