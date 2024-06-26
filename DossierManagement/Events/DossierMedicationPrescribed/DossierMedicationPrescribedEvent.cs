using DossierManagement.Common.Abstractions;
using MediatR;

namespace DossierManagement.Events.MedicationPrescribed
{
    public sealed class DossierMedicationPrescribedEvent(
        Guid patientId,
        List<string> medications)
        : Event, INotification
    {
        public Guid PatientId { get; set; } = patientId;
        public List<string> Medications { get; set; } = medications;
    }
}
