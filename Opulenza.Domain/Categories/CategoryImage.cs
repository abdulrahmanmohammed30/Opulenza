using Opulenza.Domain.Products;

namespace Opulenza.Domain.Categories;

public class CategoryImage: File
{
    public int CategoryId { get; set; }
    
    public bool IsFeaturedImage { get; set; }
}