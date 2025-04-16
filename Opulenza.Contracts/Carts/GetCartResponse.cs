namespace Opulenza.Contracts.Carts;

public class GetCartResponse
{
    public List<GetCartItemResponse> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
    public decimal? TotalPriceAfterDiscount { get; set; }
}