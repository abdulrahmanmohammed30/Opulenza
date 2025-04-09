using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Ratings.Queries.GetRatings;

public class GetRatingsQuery: PaginatedQuery, IRequest<ErrorOr<GetRatingsResult>>
{
    public int ProductId { get; init; }
    public int Rating { get; init; }
}