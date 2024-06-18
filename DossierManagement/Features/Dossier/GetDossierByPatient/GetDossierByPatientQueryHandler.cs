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
                .FirstOrDefaultAsync(d => d.Patient.Id == request.PatientId, cancellationToken);

            if (dossier == null)
                throw new ArgumentNullException($"Patient #{request.PatientId} doesn't have a dossier");

            return dossier;
        }
    }
}
