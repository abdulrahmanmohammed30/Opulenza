using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Invoices;

public class Invoice: BaseEntity
{
    public int OrderId { get; set; }
    public required string InvoiceUrl { get; set; }
}