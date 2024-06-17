using FluentValidation;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Command
{
    public sealed class CreateReferralCommandValidator
        : AbstractValidator<CreateReferralCommand>
    {
        public CreateReferralCommandValidator()
        {
            RuleFor(referralCode => referralCode.HospitalFacilityId)
                .NotEmpty()
                .WithMessage("Hospital is required!");

            RuleFor(referralCode => referralCode.BSN)
                .NotEmpty()
                .WithMessage("BSN is required!")
                .Matches(@"^\d{9}$")
                .WithMessage("BSN is invalid!");

            RuleFor(referralCode => referralCode.Diagnosis)
                .NotEmpty()
                .WithMessage("Diagnosis is required!");
        }
    }
}