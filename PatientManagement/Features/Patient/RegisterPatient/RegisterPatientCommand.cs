using MediatR;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed record RegisterPatientCommand(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string Address) 
        : IRequest;
}
