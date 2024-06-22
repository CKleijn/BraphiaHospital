using DossierManagement.Common.Aggregates;
using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Events.PatientRegistered;

namespace DossierManagement.Features.Dossier
{
    public sealed class Dossier
        : AggregateRoot
    {
        public Guid PatientId { get; set; } = Guid.Empty;
        public Patient Patient { get; set; } = null!;
        public IList<Consult>? Consults { get; set; } = new List<Consult>();
        public IList<string>? Medications { get; set; } = new List<string>();

        public void Apply(DossierCreatedEvent @event)
        {
            Id = @event.Dossier.Id;
            PatientId = @event.Dossier.PatientId;
            Patient = @event.Dossier.Patient;
            Consults = @event.Dossier.Consults;
            Medications = @event.Dossier.Medications;
        }

        public void Apply(PatientRegisteredEvent @event)
        {
            PatientId = @event.Patient.Id;
            Patient = @event.Patient;
        }

        public void Apply(DossierConsultAppendedEvent @event)
        {
            Consults ??= new List<Consult>();
            Consults.Add(@event.Consult);
        }

        public void Apply(DossierMedicationPrescribedEvent @event)
        {
            Medications ??= new List<string>();
            @event.Medications.ForEach(m => Medications.Add(m));
        }
    }
}
