using DossierManagement.Features.Dossier._Interfaces;
using DossierManagement.Features.Dossier.CreateDossier.Command;
using DossierManagement.Features.Dossier.CreateDossier.Event;
using Riok.Mapperly.Abstractions;

namespace DossierManagement.Features.Dossier
{
    [Mapper]
    public partial class DossierMapper
        : IDossierMapper
    {
        public partial Dossier CreateDossierCommandToDossier(CreateDossierCommand command);
        public partial DossierCreatedEvent DossierToDossierCreatedEvent(Dossier dossier);
    }
}
