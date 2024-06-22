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

            var aggregateEvents = await eventStore.GetAllEventsByAggregateId(dossierAggregateId, cancellationToken);
            var dossierState = new Dossier();
            dossierState.ReplayHistory(aggregateEvents);
            dossierState.Version++;

            dossierState.Consults ??= new List<Consult>();
            if (dossierState.Consults.Any(c => c.Id == notification.Consult.Id && c.PatientId == notification.Consult.PatientId))
                throw new DuplicateNameException($"Consult #{notification.Consult.Id} already exists");

            dossierState.Consults.Add(notification.Consult);

            notification.AggregateId = dossierAggregateId;

            var result = await eventStore
                   .AddEvent(
                       notification,
                       cancellationToken);

            if (!result) return;

            context
                .Set<Dossier>()
                .Update(dossierState);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
