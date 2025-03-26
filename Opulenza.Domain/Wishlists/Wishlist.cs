using Opulenza.Domain.Products;

namespace Opulenza.Domain.wishlists;

public class Wishlist 
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    public DateTime CreatedAt { get; set; }
} 