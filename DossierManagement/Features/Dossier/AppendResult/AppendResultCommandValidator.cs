using FluentValidation;

namespace DossierManagement.Features.Dossier.AppendResult
{
    public sealed class AppendResultCommandValidator
        : AbstractValidator<AppendResultCommand>
    {
        public AppendResultCommandValidator()
        {
            RuleFor(dossier => dossier.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required!");

            RuleFor(dossier => dossier.Result)
                .NotEmpty()
                .WithMessage("Result is required!");
        }
    }
}
