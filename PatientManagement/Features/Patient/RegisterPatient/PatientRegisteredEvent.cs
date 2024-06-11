using MediatR;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed record PatientRegisteredEvent(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string Address)
        : INotification;
}
