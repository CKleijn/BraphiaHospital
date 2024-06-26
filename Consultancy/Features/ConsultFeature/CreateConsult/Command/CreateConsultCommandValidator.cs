using FluentValidation;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Command
{
    public sealed class CreateConsultCommandValidator
        : AbstractValidator<CreateConsultCommand>
    {
        public CreateConsultCommandValidator()
        {
            RuleFor(consult => consult.AppointmentId)
                .NotEmpty()
                .WithMessage("AppointmentId is required!");
        }
    }
}
