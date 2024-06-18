using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Events.ResultAppended;
using DossierManagement.Features.Dossier.AppendConsult;
using DossierManagement.Features.Dossier.AppendResult;
using DossierManagement.Features.Dossier.CreateDossier;
using DossierManagement.Features.Dossier.PrescribeMedication;

namespace DossierManagement.Features.Dossier._Interfaces
{
    public interface IDossierMapper
    {
        Dossier PatientToDossier(Patient patient);
        DossierCreatedEvent DossierToDossierCreatedEvent(Dossier dossier);
        DossierConsultAppendedEvent DossierToConsultAppendedEvent(Dossier dossier);
        DossierResultAppendedEvent DossierToResultAppendedEvent(Dossier dossier);
        DossierMedicationPrescribedEvent DossierToMedicationPrescribedEvent(Dossier dossier);
    }
}
