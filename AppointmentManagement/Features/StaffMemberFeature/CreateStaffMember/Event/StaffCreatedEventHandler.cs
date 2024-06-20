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
            var result = await eventStore
                .AddEvent(
                    typeof(StaffCreatedEvent).Name,
                    JsonSerializer.Serialize(notification.StaffMember),
                    cancellationToken);

            if (!result)
                return;

            context.Set<StaffMember>().Add(notification.StaffMember);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}