using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Events.DossierCreated
{
    public sealed class DossierCreatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierCreatedEvent>
    {
        public async Task Handle(
            DossierCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            if(!await context
                .Set<Patient>()
                .AnyAsync(p => p.Id == notification.PatientId, cancellationToken))
                throw new ArgumentNullException($"Patient #{notification.PatientId} doesn't exist");

            var dossier = new Dossier
            {
                Id = notification.Id,
                PatientId = notification.PatientId
            };

            context
                .Set<Dossier>()
                .Add(dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
