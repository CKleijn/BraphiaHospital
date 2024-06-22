using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Stores;
using Microsoft.EntityFrameworkCore;
using Consultancy.Common.Helpers;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent
{
    public sealed class DossierConsultAppendedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult? consult = await context.Set<Consult>().FindAsync(notification.AggregateId, cancellationToken);
            consult?.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken));

            consult!.Version = notification.Version++;

            await context.SaveChangesAsync(cancellationToken);

            ContextDetacher.DetachAllEntitiesFromContext(context);
        }
    }
}
