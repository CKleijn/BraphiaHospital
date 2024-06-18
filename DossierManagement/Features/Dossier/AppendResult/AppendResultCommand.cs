using MediatR;

namespace DossierManagement.Features.Dossier.AppendResult
{
    public sealed record AppendResultCommand(
        Guid PatientId,
        Result Result)
        : IRequest;
}
