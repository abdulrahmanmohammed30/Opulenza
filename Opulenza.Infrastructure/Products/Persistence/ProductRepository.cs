using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task AddCategoriesToProductWithId(int productId, IEnumerable<Category> categories)
    {
        await context.Products
            .Where(p=>p.Id == productId)
            .ExecuteUpdateAsync(calls =>calls.SetProperty(product => product.Categories , categories));
    }

    public async Task<string?> GetLastSlugWithName(string slug)
    {
        return await context.Products.Where(p=>p.Slug == slug).Select(p => p.Slug)
            .OrderByDescending(s=>s).FirstOrDefaultAsync();
    }
}