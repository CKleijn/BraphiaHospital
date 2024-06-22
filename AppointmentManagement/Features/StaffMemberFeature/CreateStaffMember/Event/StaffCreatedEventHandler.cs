using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using System.Text.Json;

namespace AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event
{
    public sealed class StaffCreatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<StaffCreatedEvent>
    {
        public async Task Handle(
            StaffCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            StaffCreatedEvent staffCreatedEvent = new(notification.StaffMember)
            {
                AggregateId = notification.StaffMember.Id,
                Type = nameof(StaffCreatedEvent),
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