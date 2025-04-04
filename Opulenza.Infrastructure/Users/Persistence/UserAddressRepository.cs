using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserAddressRepository(AppDbContext context) : Repository<UserAddress>(context), IUserAddressRepository
{
    private readonly AppDbContext _context = context;

    public async Task<UserAddress?> GetByUserId(int userId, CancellationToken? cancellationToken = default)
    {
        return await _context.Set<UserAddress>().FirstOrDefaultAsync(u=>u.UserId==userId, cancellationToken.Value);    
    }
}