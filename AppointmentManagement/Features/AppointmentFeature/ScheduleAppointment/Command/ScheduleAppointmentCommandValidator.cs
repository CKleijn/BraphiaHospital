using FluentValidation;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed class ScheduleAppointmentCommandValidator
    : AbstractValidator<ScheduleAppointmentCommand>
    {
        public ScheduleAppointmentCommandValidator()
        {
            //TODO: Validate relations


            RuleFor(x => x.ScheduledDateTime)
               .NotEmpty()
               .WithMessage("ScheduledDateTime is required!");

            RuleFor(x => x.Status)
               .NotEmpty()
               .WithMessage("Status is required!")
               .IsInEnum()
               .WithMessage("Status is invalid!");
        }
    }
}
