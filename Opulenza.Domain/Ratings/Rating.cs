using Opulenza.Domain.Identity;
using Opulenza.Domain.Products;

namespace Opulenza.Domain.Ratings;

public class Rating
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Guid UserId { get; set; }

    public string? ReviewText { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}