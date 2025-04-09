namespace Opulenza.Contracts.Ratings;


public class UpdateRatingRequest
{
    public int? RatingId { get; set; }
    public int? Value { get; set; }
    public string? ReviewText { get; set; }
}
