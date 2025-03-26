using File = Opulenza.Domain.Common.File;

namespace Opulenza.Domain.Entities.Products;

public class ProductImage: File
{
    public int ProductId { get; set; }
    
    public bool IsFeaturedImage { get; set; }
}