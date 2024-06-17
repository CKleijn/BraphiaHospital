using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier.Event
{
    public sealed class PatientRegisteredEventHandler(ApplicationDbContext context)
        : INotificationHandler<PatientRegisteredEvent>
    {
        public async Task Handle(
            PatientRegisteredEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Patient>().Add(notification.Patient);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
