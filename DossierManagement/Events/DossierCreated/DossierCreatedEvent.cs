using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.DossierCreated
{
    public sealed record DossierCreatedEvent(Dossier Dossier)
        : INotification;
}
