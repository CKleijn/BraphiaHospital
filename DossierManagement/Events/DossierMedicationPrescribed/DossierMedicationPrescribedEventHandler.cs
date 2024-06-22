using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using MediatR;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed class DossierMedicationPrescribedEventHandler(
        IEventStore eventStore,
        ApplicationDbContext context)
        : INotificationHandler<DossierMedicationPrescribedEvent>
    {
        public async Task Handle(
            DossierMedicationPrescribedEvent notification,
            CancellationToken cancellationToken)
        {
            if (!await eventStore.DossierWithPatientExists(notification.PatientId, cancellationToken))
                throw new ArgumentNullException($"Dossier with patient #{notification.PatientId} doesn't exists");

            var aggregateEvents = await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken);
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
