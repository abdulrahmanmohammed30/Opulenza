namespace Opulenza.Application.Features.Ratings.Queries.GetRatings;

public class GetRatingsResult
{
    public List<GetRatingResult> Ratings { get; set; } = new();
    
    public int TotalCount { get; set; }
}