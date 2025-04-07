using Opulenza.Domain.Common;
using File = Opulenza.Domain.Common.File;

namespace Opulenza.Domain.Entities.Products;

public class ProductImage: File, IProductOwned
{
    public int ProductId { get; set; }
    
    public bool IsFeaturedImage { get; set; }
}