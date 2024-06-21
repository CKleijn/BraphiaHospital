using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Stores;

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
            Consult consult = new() { Id = notification.Consult.Id };
            consult.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Consult.Id, cancellationToken));

            consult.Version = notification.Version++;
            consult.Notes = notification.Consult.Notes;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
