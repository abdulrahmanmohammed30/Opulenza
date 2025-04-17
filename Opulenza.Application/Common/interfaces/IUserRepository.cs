using Opulenza.Application.Features.Users.Queries.GetUsers;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Common.interfaces;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<GetUsersResult> GetUsersAsync(GetUsersQuery request, CancellationToken cancellationToken);
}