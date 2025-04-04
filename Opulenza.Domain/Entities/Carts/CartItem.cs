using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Carts;

public class CartItem:BaseEntity
{
    public int CartId { get; set; }
    
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
}
