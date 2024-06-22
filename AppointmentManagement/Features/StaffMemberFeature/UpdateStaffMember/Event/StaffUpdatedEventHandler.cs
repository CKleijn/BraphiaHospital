using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Abstractions;

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
            StaffMember? staff = await context.Set<StaffMember>()
                .FindAsync(notification.Id, cancellationToken);

            StaffUpdatedEvent staffUpdatedEvent = CorrectPayload(staff!, notification);

            List<NotificationEvent> events = (await eventStore.GetAllEventsByAggregateId(notification.Id, staff!.Version, cancellationToken)).ToList();

            staffUpdatedEvent.AggregateId = staffUpdatedEvent.Id;
            staffUpdatedEvent.Type = nameof(StaffUpdatedEvent);
            staffUpdatedEvent.Payload = JsonSerializer.Serialize(staffUpdatedEvent);
            staffUpdatedEvent.Version = events.Count != 0 ? events.Last().Version + 1 : staff!.Version + 1;

            bool result = await eventStore
                .AddEvent(
                    staffUpdatedEvent,
                    cancellationToken);

            if (!result)
                return;

            staff!.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Id, staff.Version, cancellationToken));

            await context.SaveChangesAsync(cancellationToken);
        }

        private StaffUpdatedEvent CorrectPayload(StaffMember prevState, StaffUpdatedEvent input)
        {
            if (input.HospitalId == Guid.Empty)
                input.HospitalId = prevState!.HospitalId;
            if (string.IsNullOrEmpty(input.Name))
                input.Name = prevState!.Name;
            if (string.IsNullOrEmpty(input.Specialization))
                input.Specialization = prevState!.Specialization;

            return new StaffUpdatedEvent(
                prevState.Id,
                input.HospitalId,
                input.Name,
                input.Specialization
            );
        }
    }
}
