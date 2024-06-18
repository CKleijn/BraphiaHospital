using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed record DossierConsultAppendedEvent(Dossier Dossier)
        : INotification;
}
