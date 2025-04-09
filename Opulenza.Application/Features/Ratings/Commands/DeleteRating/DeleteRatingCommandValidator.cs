using FluentValidation;

namespace Opulenza.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommandValidator : AbstractValidator<DeleteRatingCommand>
{
    public DeleteRatingCommandValidator()
    {
        RuleFor(x => x.RatingId)
            .NotEmpty().WithMessage("Rating Id is required.")
            .GreaterThan(0).WithMessage("Invalid rating id.");
    }
}