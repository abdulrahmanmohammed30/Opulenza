namespace Opulenza.Contracts.Products;

public class RatingResponse
{
    public int Value { get; set; }
    public int UserId { get; set; }
    public string? ReviewText { get; set; }
}