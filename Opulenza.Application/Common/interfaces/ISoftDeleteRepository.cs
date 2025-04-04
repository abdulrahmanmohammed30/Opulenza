using Opulenza.Domain.Common;

namespace Opulenza.Application.Common.interfaces;

public interface ISoftDeleteRepository<T>  where T:ISoftDeletable, IUserOwned
{
    Task SoftDeleteByUserIdAsync(int userId);
}