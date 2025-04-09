using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommand : IRequest<ErrorOr<string>>
{
    public int? RatingId { get; set; }
}