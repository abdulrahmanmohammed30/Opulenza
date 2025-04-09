using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Domain.Entities.Ratings;

public class Rating:BaseEntity, IUserOwned, IProductOwned
{
    public int Value { get; set; }
    public int ProductId { get; set; }

    public Product Product { get; set; } 
    public int UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public string? ReviewText { get; set; }
}