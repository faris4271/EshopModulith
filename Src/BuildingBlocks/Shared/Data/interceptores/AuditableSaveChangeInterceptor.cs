using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.interceptores
{
    public class AuditableSaveChangeInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntites(eventData.Context);
            return base.SavingChanges(eventData, result);
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntites(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }



        private void UpdateEntites(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<IEntityBase>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "Faris";
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                }

                if (entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.HasChangeOwnEntities())
                {
                    entry.Entity.CreatedBy = "Faris";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }
    public static class Extension
    {
        public static bool HasChangeOwnEntities(this EntityEntry entry) =>
            entry.References.Any(r => r.TargetEntry != null
            && r.TargetEntry.Metadata.IsOwned()
            && (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

    }
}

