using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Domain.Entities.Categories;


public class Category: BaseEntity
{
    public required string Name { get; set; } 
    
    public required string Description { get; set; } 

    public required string Slug { get; set; } 

    public int? ParentId { get; set; }
    
    public List<Product>? Products { get; set; }
    
    public List<CategoryImage>? Images { get; set; }
}

