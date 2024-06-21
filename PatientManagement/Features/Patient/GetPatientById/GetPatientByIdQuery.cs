using MediatR;

namespace PatientManagement.Features.Patient.GetPatientById
{
    public sealed record GetPatientByIdQuery(Guid Id)
        : IRequest<Patient>;
}
