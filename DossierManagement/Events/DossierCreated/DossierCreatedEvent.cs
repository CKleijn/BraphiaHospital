using DossierManagement.Common.Abstractions;
using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.DossierCreated
{
    public sealed class DossierCreatedEvent(Dossier dossier)
        : Event, INotification
    {
        public Dossier Dossier { get; set; } = dossier;
    }
}
