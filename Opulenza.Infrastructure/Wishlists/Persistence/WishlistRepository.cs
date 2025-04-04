using System.Text.Unicode;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Wishlists;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Wishlists.Persistence;

public class WishlistRepository(AppDbContext context) : Repository<WishListItem>(context), IWishlistRepository;