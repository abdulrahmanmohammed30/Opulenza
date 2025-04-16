using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Carts;

public class Cart: BaseEntity, IUserOwned
{
    public int UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
    
    public decimal TotalPrice { get; set; }
    
    public decimal? TotalPriceAfterDiscount { get; set; }
}

// User -> Cart one to one 
// Cart -> CartItem is one-to-many 
// CartItem -> Product is many to one 
