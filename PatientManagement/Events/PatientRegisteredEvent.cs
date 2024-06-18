using MediatR;
using PatientManagement.Features.Patient;

namespace PatientManagement.Events
{
    public sealed record PatientRegisteredEvent(Patient Patient)
        : INotification;
}
