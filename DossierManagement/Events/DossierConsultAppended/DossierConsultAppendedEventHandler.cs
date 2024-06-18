using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed class DossierConsultAppendedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .FirstOrDefaultAsync(d => d.Patient.Id == notification.PatientId, cancellationToken)
                ?? throw new ArgumentNullException($"Dossier doesn't exist for patient #{notification.PatientId}");

            dossier.Consults!.Add(notification.Consult);

            context
                .Set<Dossier>()
                .Update(dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
