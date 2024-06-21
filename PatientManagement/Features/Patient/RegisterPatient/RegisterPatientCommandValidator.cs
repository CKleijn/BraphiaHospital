using FluentValidation;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed class RegisterPatientCommandValidator
        : AbstractValidator<RegisterPatientCommand>
    {
        public RegisterPatientCommandValidator()
        {
            RuleFor(patient => patient.FirstName)
                .NotEmpty()
                .WithMessage("First name is required!");

            RuleFor(patient => patient.LastName)
                .NotEmpty()
                .WithMessage("Last name is required!");

            RuleFor(patient => patient.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required!");

            RuleFor(patient => patient.BSN)
                .NotEmpty()
                .WithMessage("BSN is required!")
                .Matches(@"^\d{9}$")
                .WithMessage("BSN is invalid!");

            RuleFor(patient => patient.Address)
                .NotEmpty()
                .WithMessage("Address is required!");
        }
    }
}
