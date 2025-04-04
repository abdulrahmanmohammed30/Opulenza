using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Enums;

namespace Opulenza.Domain.Entities.Payments;

public class Payment:BaseEntity
{
    public PaymentStatus PaymentStatus { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    
    public int OrderId { get; set; }
}