using MediatR;

namespace PatientManagement.Features.Patient.GetPatient.Query
{
    public sealed record GetPatientQuery(Guid Id) 
        : IRequest<Patient>;
}
