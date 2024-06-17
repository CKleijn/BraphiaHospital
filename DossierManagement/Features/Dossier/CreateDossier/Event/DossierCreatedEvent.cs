using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier.Event
{
    public sealed record DossierCreatedEvent(Dossier Dossier)
        : INotification;
}
