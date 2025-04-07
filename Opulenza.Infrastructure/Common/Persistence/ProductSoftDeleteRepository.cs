using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Common;

namespace Opulenza.Infrastructure.Common.Persistence;

public class ProductSoftDeleteRepository<T>(AppDbContext context): IProductSoftDeleteRepository<T> where T: class, ISoftDeletable, IProductOwned
{
    public async Task SoftDeleteByUserIdAsync(int productId)
    {
        await context.Set<T>().Where(e => e.ProductId == productId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsDeleted, true));
    }
}
