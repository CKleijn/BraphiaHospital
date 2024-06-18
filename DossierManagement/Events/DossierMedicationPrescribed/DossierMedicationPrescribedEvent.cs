using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed record DossierMedicationPrescribedEvent(Dossier Dossier)
        : INotification;
}
