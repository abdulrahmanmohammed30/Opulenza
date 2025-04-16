using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Application.Common.interfaces;

public interface IWishlistRepository: IRepository<WishListItem>
{
    Task<List<WishListItem>> GetWishlistWithProductsAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int productId, int userId);
}