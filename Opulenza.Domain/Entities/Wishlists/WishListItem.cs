using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Domain.Entities.Wishlists;

public class WishListItem: BaseEntity, IUserOwned
{
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
    
} 