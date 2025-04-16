using Opulenza.Domain.Entities.Enums;

namespace Opulenza.Application.Features.Orders.Queries.GetOrders;

public class GetSingleOrderResult
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }
    
    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }
    
    public int? ShipmentId { get; set; }
    
    public int? PaymentId { get; set; }
    
    public PaymentStatus? PaymentStatus { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }
    public string? InvoiceUrl { get; set; }
}