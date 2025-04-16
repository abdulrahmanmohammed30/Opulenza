namespace Opulenza.Application.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistItemResult
{
    public required int WishlistItemId { get; init; }
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Slug { get; init; }

    public required decimal Price { get; init; }

    public decimal? DiscountPrice { get; init; }

    public required decimal Tax { get; init; }

    public required bool TaxIncluded { get; init; }

    public string? Brand { get; init; }

    public int? StockQuantity { get; init; }

    public required bool IsAvailable { get; init; }
}