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
                .Include(d => d.Consults)
                .Where(d => d.Id == request.Id)
                .Select(d => new Dossier
                {
                    Id = d.Id,
                    PatientId = d.PatientId,
                    Patient = d.Patient,
                    Consults = d.Consults,
                    Medications = (d.Medications ?? new List<string>()).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ArgumentNullException($"Dossier #{request.Id} doesn't exist");

            return dossier;
        }
    }
}
