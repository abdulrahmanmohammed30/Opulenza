namespace Opulenza.Contracts.Carts;

public class UpdateCartRequest
{    public List<UpdateCartItemRequest> Items { get; set; } = new(); 
}