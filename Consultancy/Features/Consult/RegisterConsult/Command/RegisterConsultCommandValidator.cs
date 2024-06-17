using FluentValidation;

namespace Consultancy.Features.Consult.RegisterConsult.Command
{
    public sealed class RegisterConsultCommandValidator
        : AbstractValidator<RegisterConsultCommand>
    {
        public RegisterConsultCommandValidator()
        {
            RuleFor(consult => consult.AppointmentId)
                .NotEmpty()
                .WithMessage("AppointmentId is required!");

            RuleFor(consult => consult.SurveyId)
                .NotEmpty()
                .WithMessage("DossierId is required!");

            RuleFor(consult => consult.SurveyId)
                .NotEmpty()
                .WithMessage("SurveyId is required!");
        }
    }
}
