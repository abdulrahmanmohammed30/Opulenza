using FluentValidation;

namespace Opulenza.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.RatingId)
            .NotEmpty().WithMessage("Rating Id is required.")
            .GreaterThan(0).WithMessage("Invalid rating id.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Value must be greater than or equal to 1.")
            .LessThanOrEqualTo(5).WithMessage("Value must be less than or equal to 5.");
    }
}