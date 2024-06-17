using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier.Event
{
    public sealed record PatientRegisteredEvent(Patient Patient)
        : INotification;
}
