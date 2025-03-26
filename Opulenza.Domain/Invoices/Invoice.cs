namespace Opulenza.Domain.Invoices;

public class Invoice
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public required string InvoiceUrl { get; set; }
}