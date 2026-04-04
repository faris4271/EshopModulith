using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.interceptores
{
    public class DispachDomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispacheDomainEvent(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispacheDomainEvent(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private async Task DispacheDomainEvent(DbContext? context)
        {
            var aggregate = context.ChangeTracker.Entries<IAggregate>()
                 .Where(a => a.Entity.Events.Any()).Select(a => a.Entity);

            var domainEvent = aggregate.SelectMany(a => a.Events).ToList();

            aggregate.ToList().ForEach(x => x.ClearDomainEvent());

            foreach (var domain in domainEvent)
            {
                await mediator.Publish(domain);
            }

        }
    }
}
