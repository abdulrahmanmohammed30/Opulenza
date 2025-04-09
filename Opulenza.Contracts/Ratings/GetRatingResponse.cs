namespace Opulenza.Contracts.Ratings;

public class GetRatingResponse
{
    public int Id { get; set; }
    public int Value { get; set; }
    public string? ReviewText { get; set; }
    public int UserId { get; set; }
    public required string  Username { get; set; }
    public string? UserProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}