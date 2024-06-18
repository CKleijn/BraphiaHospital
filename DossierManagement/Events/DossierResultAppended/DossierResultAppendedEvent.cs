using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.ResultAppended
{
    public sealed record DossierResultAppendedEvent(Dossier Dossier)
        : INotification;
}
