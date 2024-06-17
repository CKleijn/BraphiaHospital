using MediatR;

namespace PatientManagement.Features.Patient.RegisterPatient.Command
{
    public sealed record RegisterPatientCommand(
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string BSN,
        string Address)
        : IRequest;
}
