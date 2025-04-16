using Opulenza.Application.Queries.Carts;
using Opulenza.Domain.Entities.Carts;

namespace Opulenza.Application.Common.interfaces;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetTrackedCartWithItemsByUserIdAsync(int userId,CancellationToken cancellationToken = default);
    Task<Cart?> GetUntrackedCartWithItemsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<GetCartItems?> GetCartItemsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Cart?> GetTrackedCartAsync(int userId, CancellationToken cancellationToken);
}