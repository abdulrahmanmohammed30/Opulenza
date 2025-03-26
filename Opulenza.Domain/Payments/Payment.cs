using Opulenza.Domain.Enums;

namespace Opulenza.Domain.Payments;

public class Payment
{
    public int Id { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    
    public int OrderId { get; set; }
}