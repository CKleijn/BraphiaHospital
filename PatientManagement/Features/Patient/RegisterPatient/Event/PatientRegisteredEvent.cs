using MediatR;

namespace PatientManagement.Features.Patient.RegisterPatient.Event
{
    public sealed record PatientRegisteredEvent(Patient Patient)
        : INotification;
}
