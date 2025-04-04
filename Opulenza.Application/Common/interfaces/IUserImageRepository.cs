using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Common.interfaces;

public interface IUserImageRepository: IRepository<UserImage>
{
    Task<UserImage?> GetByUserIdAsync(int userId);
}