namespace Opulenza.Application.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistResult
{
    public List<GetWishlistItemResult> WishlistItems { get; set; } = new();
}