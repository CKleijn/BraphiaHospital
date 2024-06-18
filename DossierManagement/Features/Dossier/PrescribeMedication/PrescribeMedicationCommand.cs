using MediatR;

namespace DossierManagement.Features.Dossier.PrescribeMedication
{
    public sealed record PrescribeMedicationCommand(
        Guid PatientId,
        Medication Medication)
        : IRequest;
}
