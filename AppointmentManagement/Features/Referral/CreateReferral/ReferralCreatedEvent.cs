using MediatR;

namespace AppointmentManagement.Features.Referral.CreateReferral
{
    public sealed record ReferralCreatedEvent
        (
            string ReferralCode,
            string HospitalId
            //TODO: Add BSN to link to specific patient?
        ) : INotification;
}
