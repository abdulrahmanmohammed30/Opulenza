using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductImageRepository(AppDbContext context) : Repository<ProductImage>(context), IProductImageRepository
{
    public async Task<List<ProductImage>> GetImagesByProductId(int productId, CancellationToken cancellationToken)
    {
        return await context.Set<ProductImage>().Where(i => i.ProductId == productId).ToListAsync(cancellationToken);
    }
    public void AddImages(IEnumerable<ProductImage> productImages)
    {
        context.Set<ProductImage>().AddRange(productImages);
    }
}