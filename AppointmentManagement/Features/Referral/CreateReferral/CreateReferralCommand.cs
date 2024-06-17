using MediatR;

namespace AppointmentManagement.Features.Referral.CreateReferral
{
    public sealed record CreateReferralCommand(
            string ReferralCode,
            string HospitalId
        ) : IRequest;
}
