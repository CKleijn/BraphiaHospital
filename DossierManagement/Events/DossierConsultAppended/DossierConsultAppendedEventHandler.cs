using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using MediatR;
using System.Data;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed class DossierConsultAppendedEventHandler(
        IEventStore eventStore,
        ApplicationDbContext context)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification,
            CancellationToken cancellationToken)
        {
            var dossierAggregateId = await eventStore.GetDossierAggregateIdByPatientId(notification.Consult.PatientId, cancellationToken);

            if (dossierAggregateId == Guid.Empty)
                throw new ArgumentNullException($"Dossier with patient #{notification.Consult.PatientId} doesn't exists");

            notification.AggregateId = dossierAggregateId;

            var result = await eventStore
                   .AddEvent(
                       notification,
                       cancellationToken);

            if (!result) return;

            var aggregateEvents = await eventStore.GetAllEventsByAggregateId(dossierAggregateId, cancellationToken);
            var dossierState = new Dossier();
            dossierState.ReplayHistory(aggregateEvents);
            dossierState.Version++;

            context
                .Set<Dossier>()
                .Update(dossierState);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
