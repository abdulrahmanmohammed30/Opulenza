using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Carts.Persistence;

public class CartRepository: Repository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context)
    {
    }
}