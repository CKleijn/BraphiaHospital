using MediatR;

namespace DossierManagement.Features.Dossier.GetDossierByPatient
{
    public sealed record GetDossierByPatientQuery(Guid PatientId)
        : IRequest<Dossier>;
}
