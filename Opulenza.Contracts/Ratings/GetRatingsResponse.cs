namespace Opulenza.Contracts.Ratings;

public class GetRatingsResponse
{
    public List<GetRatingResponse> Ratings { get; set; } = new();
    
    public int TotalCount { get; set; }
}