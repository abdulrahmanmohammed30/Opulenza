namespace Opulenza.Domain.Carts;

public class Cart
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public List<CartItem>? Items { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public decimal TotalPriceAfterDiscount { get; set; }
}

// User -> Cart one to one 
// Cart -> CartItem is one-to-many 
// CartItem -> Product is many to one 
