using DossierManagement.Features.Dossier;
using DossierManagement.Features.Dossier.CreateDossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Events.PatientRegistered
{
    public sealed class PatientRegisteredEventHandler(
        ISender sender,
        ApplicationDbContext context)
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

            var command = new CreateDossierCommand(notification.Patient.Id);
            await sender.Send(command, cancellationToken);
        }
    }
}
