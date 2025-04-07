namespace Opulenza.Contracts.Products;

public class UpdateProductRequest
{
    public int? Id { get; set; }
    public string? Name { get; init; }
    public string? Description { get; init; } 

    public decimal? Price { get; init; }
    public decimal? DiscountPrice { get; init; }
    
    public decimal? Tax { get; init; }
    public bool? TaxIncluded { get; init; }

    public string? Brand { get; init; }
    public int? StockQuantity { get; init; }
    public List<int>? Categories { get; init; }
}