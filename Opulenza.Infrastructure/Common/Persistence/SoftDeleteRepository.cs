using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Common;

namespace Opulenza.Infrastructure.Common.Persistence;

public class SoftDeleteRepository<T>(AppDbContext context): ISoftDeleteRepository<T> where T: class, ISoftDeletable, IUserOwned
{
    public async Task SoftDeleteByUserIdAsync(int userId)
    {
        await context.Set<T>().Where(e => e.UserId == userId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsDeleted, false));
    }
}
