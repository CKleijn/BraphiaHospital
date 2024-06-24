using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using System.Text.Json;

namespace AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event
{
    public sealed class StaffMemberCreatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<StaffMemberCreatedEvent>
    {
        public async Task Handle(
            StaffMemberCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            StaffMemberCreatedEvent staffCreatedEvent = new(notification.StaffMember)
            {
                AggregateId = notification.StaffMember.Id,
                Type = nameof(StaffMemberCreatedEvent),
                Payload = JsonSerializer.Serialize(notification.StaffMember),
            };

            bool result = await eventStore
                .AddEvent(
                    staffCreatedEvent,
                    cancellationToken);

            if (!result)
                return;

            context.Set<StaffMember>().Add(notification.StaffMember);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}