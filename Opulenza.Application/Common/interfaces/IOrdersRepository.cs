using Opulenza.Application.Features.Orders.Queries.GetOrders;
using Opulenza.Domain.Entities.Orders;

namespace Opulenza.Application.Common.interfaces;

public interface IOrdersRepository: IRepository<Order>
{
    Task<List<Order>> GetOrdersAsync(GetOrdersQuery request, CancellationToken cancellationToken);
    Task<Order?> GetOrderWithItemsAsync(int id, CancellationToken cancellationToken);
}