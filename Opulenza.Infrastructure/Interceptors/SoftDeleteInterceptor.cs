using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Opulenza.Domain.Common;

namespace Opulenza.Infrastructure.Interceptors;

public class SoftDeleteInterceptor: SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        foreach (var entity in eventData.Context.ChangeTracker.Entries())
        {
            if (entity is { State: EntityState.Deleted, Entity: BaseEntity delete })
            {     
                entity.State = EntityState.Modified;
                delete.IsDeleted = true;
            }
        }
        
        return result;
    }
}