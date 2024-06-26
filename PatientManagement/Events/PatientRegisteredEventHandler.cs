using MediatR;
using PatientManagement.Features.Patient;
using PatientManagement.Infrastructure.Persistence.Contexts;
using PatientManagement.Infrastructure.Persistence.Stores;

namespace PatientManagement.Events
{
    public sealed class PatientRegisteredEventHandler(
        IEventStore eventStore,
        ApplicationDbContext context)
        : INotificationHandler<PatientRegisteredEvent>
    {
        public async Task Handle(
            PatientRegisteredEvent notification,
            CancellationToken cancellationToken)
        {
            var aggregateEvents = await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken);

            if (!aggregateEvents.Any())
                throw new ArgumentNullException($"Patient #{notification.Patient.Id} doesn't exists");

            var patientState = new Patient();
            patientState.ReplayHistory(aggregateEvents);

            context
                .Set<Patient>()
                .Add(patientState);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
