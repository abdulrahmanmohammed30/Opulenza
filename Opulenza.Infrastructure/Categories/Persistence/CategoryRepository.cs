using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Categories.Persistence;

public class CategoryRepository(AppDbContext context): Repository<Category>(context), ICategoryRepository
{
    public async Task<List<Category>> GetCategories(IEnumerable<int> categoryIds)
    {
        return await context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
    }
}