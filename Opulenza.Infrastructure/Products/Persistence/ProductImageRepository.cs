using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductImageRepository : Repository<ProductImage>
{
    public ProductImageRepository(AppDbContext context) : base(context)
    {
    }
}