using DossierManagement.Common.Helpers;
using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using MediatR;

namespace DossierManagement.Events.DossierCreated
{
    public sealed class DossierCreatedEventHandler(
        IEventStore eventStore,
        ApplicationDbContext context)
        : INotificationHandler<DossierCreatedEvent>
    {
        public async Task Handle(
            DossierCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            var aggregateEvents = await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken);

            var dossierState = new Dossier();
            dossierState.ReplayHistory(aggregateEvents);

            context
                .Set<Dossier>()
                .Add(dossierState);

            await context.SaveChangesAsync(cancellationToken);

            ContextDetacher.DetachAllEntitiesFromContext(context);
        }
    }
}
