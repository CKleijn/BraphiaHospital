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
                .Where(d => d.PatientId == request.PatientId)
                .Select(d => new Dossier
                {
                    Id = d.Id,
                    PatientId = d.PatientId,
                    Patient = d.Patient,
                    Consults = d.Consults,
                    Medications = (d.Medications ?? new List<string>()).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ArgumentNullException($"Patient #{request.PatientId} doesn't have a dossier");

            return dossier;
        }
    }
}
