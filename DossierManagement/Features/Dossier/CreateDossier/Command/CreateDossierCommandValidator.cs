using FluentValidation;

namespace DossierManagement.Features.Dossier.CreateDossier.Command
{
    public sealed class CreateDossierCommandValidator
        : AbstractValidator<CreateDossierCommand>
    {
        public CreateDossierCommandValidator()
        {
            RuleFor(dossier => dossier.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required!");
        }
    }
}
