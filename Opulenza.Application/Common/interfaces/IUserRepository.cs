using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Common.interfaces;

public interface IUserRepository: IRepository<ApplicationUser>
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}