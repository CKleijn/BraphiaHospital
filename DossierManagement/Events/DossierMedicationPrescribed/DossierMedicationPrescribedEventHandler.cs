using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed class DossierMedicationPrescribedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierMedicationPrescribedEvent>
    {
        public async Task Handle(
            DossierMedicationPrescribedEvent notification,
            CancellationToken cancellationToken)
        {
            context
                .Set<Dossier>()
                .Update(notification.Dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
