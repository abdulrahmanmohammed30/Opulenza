using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Enums;

namespace Opulenza.Domain.Entities.Orders;

public class Order: BaseEntity, IOptionalUserOwned
{ 
    public int? UserId { get; set; }
    
    // required at the database level, an order must have at least one item 
    public List<OrderItem>? Items { get; set; } 
    
    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }
    
    public int PaymentId { get; set; }
    
    public int? ShipmentId { get; set; }
}   
