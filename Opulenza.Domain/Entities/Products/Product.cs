using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Ratings;

namespace Opulenza.Domain.Entities.Products;

public class Product: BaseEntity
{
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

    public List<ProductImage>? Images { get; set; }

    public List<Rating>? Ratings { get; set; } 

    public List<Category>? Categories { get; set; }
    
}