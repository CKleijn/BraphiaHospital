using FluentValidation;

namespace DossierManagement.Features.Dossier.AppendConsult
{
    public sealed class AppendConsultCommandValidator
        : AbstractValidator<AppendConsultCommand>
    {
        public AppendConsultCommandValidator()
        {
            RuleFor(dossier => dossier.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required!");

            RuleFor(dossier => dossier.Consult)
                .NotEmpty()
                .WithMessage("Consult is required!");
        }
    }
}
