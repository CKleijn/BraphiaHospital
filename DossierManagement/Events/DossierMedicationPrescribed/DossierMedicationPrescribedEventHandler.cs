using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed class DossierMedicationPrescribedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierMedicationPrescribedEvent>
    {
        public async Task Handle(
            DossierMedicationPrescribedEvent notification,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .FirstOrDefaultAsync(d => d.PatientId == notification.PatientId, cancellationToken)
                ?? throw new ArgumentNullException($"Dossier doesn't exist for patient #{notification.PatientId}");

            dossier.Medications ??= new List<string>();
            notification.Medications.ForEach(m => dossier.Medications.Add(m));

            context
                .Set<Dossier>()
                .Update(dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
