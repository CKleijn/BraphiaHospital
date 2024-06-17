using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier.Command
{
    public sealed record CreateDossierCommand(Guid PatientId)
        : IRequest;
}
