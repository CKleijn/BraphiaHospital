using MediatR;
using PatientManagement.Common.Abstractions;
using PatientManagement.Features.Patient;

namespace PatientManagement.Events
{
    public sealed class PatientRegisteredEvent(Patient patient)
        : Event, INotification
    {
        public Patient Patient { get; set; } = patient;
    }
}
