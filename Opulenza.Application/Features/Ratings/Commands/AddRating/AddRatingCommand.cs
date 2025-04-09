using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Ratings.Commands.AddRating;

public class AddRatingCommand: IRequest<ErrorOr<int>>
{
    public int? Value { get; set; }
    public int? ProductId { get; set; }
    public string? ReviewText { get; set; }
}


