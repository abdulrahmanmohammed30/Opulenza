using Opulenza.Domain.Enums;
namespace Opulenza.Domain.Orders;

public class Order
{ 
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    // required at the database level, an order must have at least one item 
    public List<OrderItem>? Items { get; set; } 
    
    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    public int PaymentId { get; set; }
}   
