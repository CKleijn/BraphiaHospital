using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Command;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;

namespace AppointmentManagement.Common.Interfaces
{
    public interface IReferralMapper
    {
        Referral CreatedReferralCommandToReferral(CreateReferralCommand command);
        ReferralCreatedEvent ReferralToReferralCreatedEvent(Referral referral);
    }
}