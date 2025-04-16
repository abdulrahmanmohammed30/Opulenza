using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Enums;
using Opulenza.Domain.Entities.Invoices;
using Opulenza.Domain.Entities.Payments;

namespace Opulenza.Domain.Entities.Orders;

public class Order: BaseEntity, IOptionalUserOwned
{ 
    public int? UserId { get; set; }
    
    // required at the database level, an order must have at least one item 
    public List<OrderItem>? Items { get; set; } 
    
    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }
    
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    
    // the shipment is optional, since some products are digital 
    // also optional since the shipment company may take a while before providing the tracking number
    public int? ShipmentId { get; set; }
    
    public Invoice? Invoice { get; set; }
}   
