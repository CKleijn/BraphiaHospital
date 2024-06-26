using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Abstractions;
using Azure.Core;

namespace AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event
{
    public sealed class StaffMemberUpdatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<StaffMemberUpdatedEvent>
    {
        public async Task Handle(
            StaffMemberUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            StaffMember? staff = await context.Set<StaffMember>()
                .FindAsync(notification.Id, cancellationToken);

            if (staff == null)
                return;

            StaffMemberUpdatedEvent staffUpdatedEvent = CorrectPayload(staff!, notification);

            List<NotificationEvent> events = (await eventStore.GetAllEventsByAggregateId(notification.Id, staff!.Version, cancellationToken)).ToList();

            staffUpdatedEvent.AggregateId = staffUpdatedEvent.Id;
            staffUpdatedEvent.Type = nameof(StaffMemberUpdatedEvent);
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

        private StaffMemberUpdatedEvent CorrectPayload(StaffMember prevState, StaffMemberUpdatedEvent input)
        {
            if (input.HospitalId == Guid.Empty)
                input.HospitalId = prevState!.HospitalId;
            if (string.IsNullOrEmpty(input.Name))
                input.Name = prevState!.Name;
            if (string.IsNullOrEmpty(input.Specialization))
                input.Specialization = prevState!.Specialization;

            return new StaffMemberUpdatedEvent(
                prevState.Id,
                input.HospitalId,
                input.Name,
                input.Specialization
            );
        }
    }
}
