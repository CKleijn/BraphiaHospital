using MediatR;

namespace PatientManagement.Features.Patient.GetPatients.Query
{
    public sealed record GetPatientsQuery() 
        : IRequest<IEnumerable<Patient>>;
}
