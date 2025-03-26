using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Orders;

public class OrderItem: Entity
{
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TotalPrice  { get; set; }

    public int Quantity { get; set; }
    
    public decimal Tax { get; set; }
    
    public bool TaxIncluded { get; set; }
}