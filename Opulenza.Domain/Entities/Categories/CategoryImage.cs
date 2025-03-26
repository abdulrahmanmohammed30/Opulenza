using File = Opulenza.Domain.Common.File;

namespace Opulenza.Domain.Entities.Categories;

public class CategoryImage: File
{
    public int CategoryId { get; set; }
    
    public bool IsFeaturedImage { get; set; }
}