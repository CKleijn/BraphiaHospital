using DossierManagement.Features.Dossier.CreateDossier.Command;
using DossierManagement.Features.Dossier.CreateDossier.Event;

namespace DossierManagement.Features.Dossier._Interfaces
{
    public interface IDossierMapper
    {
        Dossier CreateDossierCommandToDossier(CreateDossierCommand command);
        DossierCreatedEvent DossierToDossierCreatedEvent(Dossier dossier);
    }
}
