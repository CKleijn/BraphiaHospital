using AppointmentManagement.Common.Entities;
using FluentValidation;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed class ScheduleAppointmentCommandValidator
    : AbstractValidator<ScheduleAppointmentCommand>
    {
        public ScheduleAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required!");

            RuleFor(x => x.ReferralId)
                .NotEmpty()
                .WithMessage("ReferralId is required!");

            RuleFor(x => x.PhysicianId)
                .NotEmpty()
                .WithMessage("PhysicianId is required!");

            RuleFor(x => x.HospitalFacilityId)
                .NotEmpty()
                .WithMessage("HospitalFacilityId is required!");

            RuleFor(x => x.ScheduledDateTime)
               .NotEmpty()
               .WithMessage("ScheduledDateTime is required!");
        }
    }
}
