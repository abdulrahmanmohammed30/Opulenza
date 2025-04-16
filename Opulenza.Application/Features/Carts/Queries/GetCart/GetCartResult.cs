namespace Opulenza.Application.Features.Carts.Queries.GetCart;

public class GetCartResult
{
    public List<GetCartItemResult> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
    public decimal? TotalPriceAfterDiscount { get; set; }
}