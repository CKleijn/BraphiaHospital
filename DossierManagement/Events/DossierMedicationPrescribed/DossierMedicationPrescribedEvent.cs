using MediatR;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed record DossierMedicationPrescribedEvent(
        Guid PatientId,
        List<string> Medications)
        : INotification;
}
