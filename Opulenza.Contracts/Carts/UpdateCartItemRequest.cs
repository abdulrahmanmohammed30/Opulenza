namespace Opulenza.Contracts.Carts;

public class UpdateCartItemRequest
{
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}