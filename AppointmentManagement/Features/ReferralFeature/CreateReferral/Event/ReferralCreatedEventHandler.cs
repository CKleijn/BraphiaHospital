using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Event
{
    public sealed class ReferralCreatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<ReferralCreatedEvent>
    {
        public async Task Handle(
            ReferralCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            context.Set<Referral>().Add(notification.Referral);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}