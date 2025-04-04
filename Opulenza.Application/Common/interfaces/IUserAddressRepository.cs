using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Common.interfaces;

public interface IUserAddressRepository: IRepository<UserAddress>
{
    Task<UserAddress?> GetByUserId(int userId, CancellationToken? cancellationToken = default); 
}