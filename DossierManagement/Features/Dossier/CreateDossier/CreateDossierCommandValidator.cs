using FluentValidation;

namespace DossierManagement.Features.Dossier.CreateDossier
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
