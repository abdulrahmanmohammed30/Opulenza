using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserImageRepository: Repository<UserImage>, IUserImageRepository
{
    private readonly AppDbContext _context;

    public UserImageRepository(AppDbContext context): base(context)
    {
        _context = context;
    }

    public async Task<UserImage?> GetByUserIdAsync(int userId)
    {
        return await _context.Set<UserImage>().Where(u=> u.UserId == userId).FirstOrDefaultAsync();
    }
}