namespace Opulenza.Contracts.Products;

public class AddProductRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; } 

    public required decimal Price { get; init; }
    public decimal? DiscountPrice { get; init; }
    public decimal Tax { get; init; } = 0;
    public bool TaxIncluded { get; init; } = false;

    public string? Brand { get; init; }
    public int? StockQuantity { get; init; }
    public bool IsAvailable { get; init; }
    
    public List<int>? Categories { get; set; }
}