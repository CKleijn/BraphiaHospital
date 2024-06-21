using MediatR;
using PatientManagement.Features.Patient;
using PatientManagement.Infrastructure.Persistence.Contexts;
using PatientManagement.Infrastructure.Persistence.Stores;
using System.Data;

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
            if (!await eventStore.EventByAggregateIdExists(notification.AggregateId, cancellationToken))
                throw new DuplicateNameException($"Patient #{notification.AggregateId} already exists");

            context
                .Set<Patient>()
                .Add(notification.Patient);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
