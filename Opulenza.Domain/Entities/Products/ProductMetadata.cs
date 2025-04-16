using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Products;

public class ProductMetadata: BaseEntity
{
    public int ProductId { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
    public Product? Product { get; set; }
}

