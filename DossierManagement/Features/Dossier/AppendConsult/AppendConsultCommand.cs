using MediatR;

namespace DossierManagement.Features.Dossier.AppendConsult
{
    public sealed record AppendConsultCommand(
        Guid PatientId, 
        Consult Consult)
        : IRequest;
}
