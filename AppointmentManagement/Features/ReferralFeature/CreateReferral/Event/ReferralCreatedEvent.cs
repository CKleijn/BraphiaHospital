using MediatR;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Common.Abstractions;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Event
{
    public sealed class ReferralCreatedEvent(Referral referral)
        : NotificationEvent, INotification
    {
        public Referral Referral { get; set; } = referral;
    }
}
