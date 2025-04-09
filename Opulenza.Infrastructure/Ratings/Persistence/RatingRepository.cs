using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Ratings.Queries.GetRatings;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Ratings.Persistence;

public class RatingRepository(AppDbContext context) : Repository<Rating>(context), IRatingRepository
{
    public async Task<bool> ExistsAsync(int userId, int productId, CancellationToken cancellationToken)
    {
        return await context.Set<Rating>()
            .AnyAsync(r => r.UserId == userId && r.ProductId == productId, cancellationToken);
    }

    public async Task<List<GetRatingResult>> GetRatingsByProductIdAsync(GetRatingsQuery query,
        CancellationToken cancellationToken)
    {
        return await context.Set<Rating>()
            .Where(r => r.ProductId == query.ProductId && r.Value == query.Rating)
            .OrderBy(r=>r.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(r => new GetRatingResult()
            {
                Id = r.Id,
                Value = r.Value,
                ReviewText = r.ReviewText,
                CreatedAt = r.CreatedAt,
                UserId = r.UserId,
                Username = r.User.UserName,
                UserProfileUrl = r.User.Image != null ? r.User.Image.FilePath : null
            }).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(int productId, int rating, CancellationToken cancellationToken)
    {
        return await context.Set<Rating>()
            .CountAsync(r => r.ProductId == productId && r.Value == rating, cancellationToken);
    }
}