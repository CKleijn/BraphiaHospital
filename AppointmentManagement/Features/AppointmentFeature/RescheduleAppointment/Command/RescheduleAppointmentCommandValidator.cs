using FluentValidation;

namespace AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command
{
    public sealed class RescheduleAppointmentCommandValidator
    : AbstractValidator<RescheduleAppointmentCommand>
    {
        public RescheduleAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required!");

            RuleFor(x => x.ScheduledDateTime)
                .NotEmpty()
                .WithMessage("ScheduledDateTime is required!");
        }
    }
}
