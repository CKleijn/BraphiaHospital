using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Features.Patient;
using PatientManagement.Infrastructure.Persistence.Contexts;
using System.Data;

namespace PatientManagement.Events
{
    public sealed class PatientRegisteredEventHandler(ApplicationDbContext context)
        : INotificationHandler<PatientRegisteredEvent>
    {
        public async Task Handle(
            PatientRegisteredEvent notification,
            CancellationToken cancellationToken)
        {
            if (await context
                .Set<Patient>()
                .AnyAsync(p => p.Id == notification.Patient.Id, cancellationToken))
                throw new DuplicateNameException($"Patient #{notification.Patient.Id} already exists");

            context
                .Set<Patient>()
                .Add(notification.Patient);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
