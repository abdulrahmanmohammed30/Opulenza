using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Ratings;

public class GetRatingsRequest: PaginatedRequest
{
    public int Rating { get; init; }
}