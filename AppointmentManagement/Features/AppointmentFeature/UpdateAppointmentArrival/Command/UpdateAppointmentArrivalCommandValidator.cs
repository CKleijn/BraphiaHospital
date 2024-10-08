﻿using FluentValidation;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command
{
    public sealed class UpdateAppointmentArrivalCommandValidator
    : AbstractValidator<UpdateAppointmentArrivalCommand>
    {
        public UpdateAppointmentArrivalCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required!");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required!")
                .IsInEnum()
                .WithMessage("Status is invalid!");
        }
    }
}
