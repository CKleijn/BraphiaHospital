using MediatR;

namespace DossierManagement.Features.Dossier.GetDossierById
{
    public sealed record GetDossierByIdQuery(Guid Id)
        : IRequest<Dossier>;
}
