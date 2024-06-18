using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.PatientRegistered
{
    public sealed record PatientRegisteredEvent(Patient Patient)
        : INotification;
}
