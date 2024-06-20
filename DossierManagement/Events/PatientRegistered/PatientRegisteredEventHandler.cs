using DossierManagement.Features.Dossier;
using DossierManagement.Features.Dossier.CreateDossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            if (await context
                .Set<Patient>()
                .AnyAsync(p => p.Id == notification.Patient.Id, cancellationToken))
                throw new DuplicateNameException($"Patient #{notification.Patient.Id} already exists");

            context
                .Set<Patient>()
                .Add(notification.Patient);

            await context.SaveChangesAsync(cancellationToken);

            var command = new CreateDossierCommand(notification.Patient.Id);
            await sender.Send(command, cancellationToken);
        }
    }
}
