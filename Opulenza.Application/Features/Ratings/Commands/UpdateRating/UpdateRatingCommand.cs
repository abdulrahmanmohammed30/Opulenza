using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommand: IRequest<ErrorOr<string>>
{
    public int? RatingId { get; set; }
    public int? Value { get; set; }
    public string? ReviewText { get; set; }
}

