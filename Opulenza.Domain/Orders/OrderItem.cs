using Opulenza.Domain.Products;

namespace Opulenza.Domain.Orders;

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TotalPrice  { get; set; }

    public int Quantity { get; set; }
    
    public decimal Tax { get; set; }
    
    public bool TaxIncluded { get; set; }
}