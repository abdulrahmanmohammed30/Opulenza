namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductListItemResult
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
}