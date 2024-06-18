using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Events.ResultAppended;
using DossierManagement.Features.Dossier._Interfaces;
using Riok.Mapperly.Abstractions;

namespace DossierManagement.Features.Dossier
{
    [Mapper]
    public partial class DossierMapper
        : IDossierMapper
    {
        public partial Dossier PatientToDossier(Patient patient);
        public partial DossierCreatedEvent DossierToDossierCreatedEvent(Dossier dossier);
        public partial DossierConsultAppendedEvent DossierToConsultAppendedEvent(Dossier dossier);
        public partial DossierResultAppendedEvent DossierToResultAppendedEvent(Dossier dossier);
        public partial DossierMedicationPrescribedEvent DossierToMedicationPrescribedEvent(Dossier dossier);
    }
}
