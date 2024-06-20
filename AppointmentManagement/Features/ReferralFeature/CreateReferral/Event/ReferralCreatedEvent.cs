using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Event
{
    public sealed record ReferralCreatedEvent(Referral Referral)
        : INotification;
}
