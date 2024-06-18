using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Features.Dossier.GetDossierById
{
    public sealed class GetDossierByIdQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetDossierByIdQuery, Dossier>
    {
        public async Task<Dossier> Handle(
            GetDossierByIdQuery request,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (dossier == null)
                throw new ArgumentNullException($"Dossier #{request.Id} doesn't exist");

            return dossier;
        }
    }
}
