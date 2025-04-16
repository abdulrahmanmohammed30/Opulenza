using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Enums;
using Opulenza.Domain.Entities.Orders;

namespace Opulenza.Domain.Entities.Payments;

public class Payment:BaseEntity
{
    public PaymentStatus PaymentStatus { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    
    public int OrderId { get; set; }
    public Order? Order { get; set; }
}