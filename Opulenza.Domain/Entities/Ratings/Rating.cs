using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Domain.Entities.Ratings;

public class Rating:BaseEntity, IUserOwned, IProductOwned
{
    public int Value { get; set; }
    public int ProductId { get; set; }

    public Product Product { get; set; } 
    public int UserId { get; set; }

    public string? ReviewText { get; set; }
}