using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Products;

public class ProductResponse
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Slug { get; init; }

    public decimal Price { get; init; }

    public decimal? DiscountPrice { get; init; }

    public decimal Tax { get; init; }

    public bool TaxIncluded { get; init; }

    public string? Brand { get; init; }

    public int? StockQuantity { get; init; }

    public bool IsAvailable { get; init; }

    public List<ImageResponse>? Images { get; init; }

    public List<RatingResponse>? Ratings { get; init; }

    public List<CategoryResponse>? Categories { get; init; }
}