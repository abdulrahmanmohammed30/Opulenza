using Opulenza.Domain.Categories;
using Opulenza.Domain.Ratings;

namespace Opulenza.Domain.Products;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }
    
    public required string Description { get; set; } 

    public required string Slug { get; set; }
    
    public decimal Price { get; set; }

    public decimal DiscountPrice { get; set; }

    public decimal Tax { get; set; }

    public bool TaxIncluded { get; set; }
 
    public string? Brand { get; set; }

    public int? StockQuantity { get; set; }

    public bool IsAvailable { get; set; }

    public List<ProductImage>? Images { get; set; }

    public List<Category>? Categories { get; set; }

    public List<Rating>? Ratings { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}