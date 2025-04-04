using Opulenza.Domain.Common;

namespace Opulenza.Application.Common.interfaces;

public interface IOptionalSoftDeleteRepository<T>  where T:ISoftDeletable, IOptionalUserOwned
{
    Task SoftDeleteByUserIdAsync(int userId);
}
