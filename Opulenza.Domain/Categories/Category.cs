using Opulenza.Domain.Products;

namespace Opulenza.Domain.Categories;



public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; } 
    
    public required string Description { get; set; } 

    public required string Slug { get; set; } 

    public int? ParentId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public List<Product>? Products { get; set; }
    
    public List<CategoryImage>? Images { get; set; }
}

