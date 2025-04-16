using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Domain.Entities.Carts;

public class CartItem:BaseEntity
{
    public int CartId { get; set; }
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int Quantity { get; set; }
}
