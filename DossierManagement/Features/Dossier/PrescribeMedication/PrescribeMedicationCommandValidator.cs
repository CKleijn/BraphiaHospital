using FluentValidation;

namespace DossierManagement.Features.Dossier.PrescribeMedication
{
    public sealed class PrescribeMedicationCommandValidator
        : AbstractValidator<PrescribeMedicationCommand>
    {
        public PrescribeMedicationCommandValidator()
        {
            RuleFor(dossier => dossier.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required!");

            RuleFor(dossier => dossier.Medication)
                .NotEmpty()
                .WithMessage("Medication is required!");
        }
    }
}
