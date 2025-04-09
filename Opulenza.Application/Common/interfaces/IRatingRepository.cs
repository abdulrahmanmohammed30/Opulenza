using Opulenza.Application.Features.Ratings.Queries.GetRatings;
using Opulenza.Domain.Entities.Ratings;

namespace Opulenza.Application.Common.interfaces;

public interface IRatingRepository: IRepository<Rating>
{
    Task<bool> ExistsAsync(int userId, int productId, CancellationToken cancellationToken);
    Task<List<GetRatingResult>> GetRatingsByProductIdAsync(GetRatingsQuery query,
        CancellationToken cancellationToken);
    Task<int> CountAsync(int productId, int rating, CancellationToken cancellationToken);
}