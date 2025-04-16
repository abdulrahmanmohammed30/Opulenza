using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Categories.Persistence;

public class CategoryImageRepository(AppDbContext context): Repository<CategoryImage>(context), ICategoryImageRepository
{
    public async Task<List<CategoryImage>> GetImagesByCategoryId(int categoryId, CancellationToken cancellationToken)
    {
        return await context.Set<CategoryImage>().Where(i => i.CategoryId == categoryId).ToListAsync(cancellationToken);
    }
    
    public void AddImages(IEnumerable<CategoryImage> categoryImages)
    {
        context.Set<CategoryImage>().AddRange(categoryImages);
    }
}
