using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Features.Dossier.GetDossierByPatient
{
    public sealed class GetDossierByPatientQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetDossierByPatientQuery, Dossier>
    {
        public async Task<Dossier> Handle(
            GetDossierByPatientQuery request,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .Include(d => d.Patient)
                .Include(d => d.Consults)
                .FirstOrDefaultAsync(d => d.PatientId == request.PatientId, cancellationToken)
                ?? throw new ArgumentNullException($"Patient #{request.PatientId} doesn't have a dossier");

            return dossier;
        }
    }
}
