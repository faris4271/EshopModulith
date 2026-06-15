using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Contract.Context;
using Shared.DDD;

namespace Shared.Data.interceptores
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUser currentUser;

        public AuditableEntityInterceptor(ICurrentUser currentUser)
        {
            this.currentUser = currentUser;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                var now = DateTimeOffset.UtcNow;

                // Ensure userId is a string (handles Guid or other types returned by GetUserId)
                var userId = currentUser.GetUserId().ToString();

                if (userId == Guid.Empty.ToString())
                {
                    userId = "System"; // or any default value for unauthenticated actions
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedById).CurrentValue = userId;
                    entry.Property(x => x.CreatedOn).CurrentValue = now;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Property(x => x.LatestUpdatedById).CurrentValue = userId;
                    entry.Property(x => x.LatestUpdatedOn).CurrentValue = now;
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}