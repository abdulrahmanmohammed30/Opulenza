namespace Opulenza.Domain.Entities.Categories;

public class CategoryImageRepository(AppContext context): Repository< ICategoryImageRepository
{
    public async Task<List<CategoryImage>> GetImagesByProductId(int productId, CancellationToken cancellationToken)
    {
        return await context.Set<CategoryImage>().Where(i => i.ProductId == productId).ToListAsync(cancellationToken);
    }
    public void AddImages(IEnumerable<ProductImage> productImages)
    {
        context.Set<ProductImage>().AddRange(productImages);
    }
}