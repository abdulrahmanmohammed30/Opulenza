namespace Opulenza.Contracts.Wishlist;

public class GetWishlistResponse
{
    public List<GetWishlistItemResponse> WishlistItems { get; set; } = new();
}