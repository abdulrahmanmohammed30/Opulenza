using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Wishlists;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Wishlists.Persistence;

public class WishlistRepository(AppDbContext context) : Repository<WishListItem>(context), IWishlistRepository
{
    public async Task<List<WishListItem>> GetWishlistWithProductsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var result = await context.WishlistItems.Include(p=>p.Product).Where(x => x.UserId == userId)
       .ToListAsync(cancellationToken);

        return result.Select(x => x!).ToList();
    }

    public async Task<bool> ExistsAsync(int productId, int userId)
    {
        return await context.WishlistItems.AnyAsync(x => x.ProductId == productId && x.UserId == userId);
    }
}