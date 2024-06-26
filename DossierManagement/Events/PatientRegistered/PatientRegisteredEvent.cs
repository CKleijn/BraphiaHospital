using DossierManagement.Common.Abstractions;
using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.PatientRegistered
{
    public sealed class PatientRegisteredEvent(Patient patient)
        : Event, INotification
    {
        public Patient Patient { get; set; } = patient;
    }
}
