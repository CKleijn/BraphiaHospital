using MediatR;

namespace PatientManagement.Features.Patient.GetPatients
{
    public sealed record GetPatientsQuery()
        : IRequest<IEnumerable<Patient>>;
}
