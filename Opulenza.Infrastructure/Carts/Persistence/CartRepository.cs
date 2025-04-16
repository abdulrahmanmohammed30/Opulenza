using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Queries.Carts;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Carts.Persistence;

public class CartRepository(AppDbContext context) : Repository<Cart>(context), ICartRepository
{
    public async Task<Cart?> GetTrackedCartWithItemsByUserIdAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Carts.AsTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<Cart?> GetUntrackedCartWithItemsByUserIdAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<GetCartItems?> GetCartItemsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await context.Carts
            .Where(p => p.UserId == userId).Select(c => new GetCartItems()
            {
                Items = c.Items.Select(x => new GetCartItem()
                {
                    ProductId = x.ProductId,
                    Price = x.Product.Price,
                    Quantity = x.Quantity,
                    PaymentServiceId = x.Product.PaymentServiceId
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<Cart?> GetTrackedCartAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Carts.AsTracking()
            .Include(x=>x.Items)
            .ThenInclude(item=>item.Product)
            .Where(c=>c.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}