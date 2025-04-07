using Opulenza.Domain.Common;

namespace Opulenza.Application.Common.interfaces;

public interface IProductSoftDeleteRepository<T>  where T:ISoftDeletable, IProductOwned
{
    Task SoftDeleteByUserIdAsync(int userId);
}
