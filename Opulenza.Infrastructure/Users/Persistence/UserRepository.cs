using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserRepository(AppDbContext context) : Repository<ApplicationUser>(context), IUserRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return context.Users.AnyAsync(u => u.Id == id, cancellationToken);
    }
}