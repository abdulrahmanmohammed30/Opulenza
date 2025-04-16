namespace Opulenza.Contracts.Orders;

public class GetOrderResponse
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }
    
    public required List<GetOrderItemResponse> Items { get; set; } 
    
    public decimal TotalAmount { get; set; }

    public required string OrderStatus { get; set; }
    
    public int? ShipmentId { get; set; }
    
    public int? PaymentId { get; set; }
    
    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }
    public string? InvoiceUrl { get; set; }
}