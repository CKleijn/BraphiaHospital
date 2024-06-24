using MediatR;

namespace PatientManagement.Features.Patient.SyncPatients
{
    public sealed record SyncPatientsCommand()
        : IRequest;
}
