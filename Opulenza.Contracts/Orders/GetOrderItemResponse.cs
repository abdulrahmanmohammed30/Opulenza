namespace Opulenza.Contracts.Orders;

public class GetOrderItemResponse
{
    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TotalPrice  { get; set; }

    public int Quantity { get; set; }
    
    public decimal Tax { get; set; }
    
    public bool TaxIncluded { get; set; }
}