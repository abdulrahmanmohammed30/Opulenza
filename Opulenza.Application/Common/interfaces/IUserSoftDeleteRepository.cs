using Opulenza.Domain.Common;

namespace Opulenza.Application.Common.interfaces;

public interface IUserSoftDeleteRepository<T>  where T:ISoftDeletable, IUserOwned
{
    Task SoftDeleteByUserIdAsync(int userId);
}
