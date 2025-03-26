using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Ratings;

public class Rating:BaseEntity
{
    public int Value { get; set; }
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string? ReviewText { get; set; }
}