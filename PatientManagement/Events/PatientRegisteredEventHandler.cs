using MediatR;
using PatientManagement.Features.Patient;
using PatientManagement.Infrastructure.Persistence.Contexts;

namespace PatientManagement.Events
{
    public sealed class PatientRegisteredEventHandler(ApplicationDbContext context)
        : INotificationHandler<PatientRegisteredEvent>
    {
        public async Task Handle(
            PatientRegisteredEvent notification,
            CancellationToken cancellationToken)
        {
            context
                .Set<Patient>()
                .Add(notification.Patient);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
