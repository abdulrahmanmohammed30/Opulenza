using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Orders.Queries.GetOrders;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Orders.Persistence;

public class OrdersRepository(AppDbContext context): Repository<Order>(context), IOrdersRepository
{
    public async Task<List<Order>> GetOrdersAsync(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await context.Orders
            .Include(o=>o.Payment)
            .Include(o=>o.Invoice)
            .OrderBy(x => x.Id).Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderWithItemsAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Orders
            .Include(o => o.Payment)
            .Include(o => o.Invoice)
            .Include(o=>o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}