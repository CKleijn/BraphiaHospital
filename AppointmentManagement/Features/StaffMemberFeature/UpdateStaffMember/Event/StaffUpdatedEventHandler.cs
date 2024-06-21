using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Helpers;

namespace AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event
{
    public sealed class StaffUpdatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<StaffUpdatedEvent>
    {
        public async Task Handle(
            StaffUpdatedEvent notification,
            CancellationToken cancellationToken)
        {

            List<NotificationEvent> events = (await eventStore.GetAllEventsByAggregateId(notification.Id, cancellationToken)).ToList();

            int newVersion = Utils.GetHighestVersionByType<StaffUpdatedEvent>(events) + 1;

            StaffUpdatedEvent staffUpdatedEvent = new(
                notification.Id,
                notification.HospitalId,
                notification.Name,
                notification.Specialization)
            {
                AggregateId = notification.Id,
                Type = nameof(StaffUpdatedEvent),
                Payload = JsonSerializer.Serialize(notification),
                Version = newVersion
            };

            bool result = await eventStore
                .AddEvent(
                    staffUpdatedEvent,
                    cancellationToken);

            if (!result)
                return;

            StaffMember? staff = await context.Set<StaffMember>()
                .FindAsync(notification.Id, cancellationToken);

            staff!.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Id, cancellationToken));

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
