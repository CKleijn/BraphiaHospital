using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event;
using AppointmentManagement.Common.Helpers;

namespace AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event
{
    public sealed class HospitalFacilityUpdatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<HospitalFacilityUpdatedEvent>
    {
        public async Task Handle(
            HospitalFacilityUpdatedEvent notification,
            CancellationToken cancellationToken)
        {

            List<NotificationEvent> events = (await eventStore.GetAllEventsByAggregateId(notification.Id, cancellationToken)).ToList();

            int newVersion = Utils.GetHighestVersionByType<HospitalFacilityUpdatedEvent>(events) + 1;

            HospitalFacilityUpdatedEvent hospitalFacilityUpdatedEvent = new(
                notification.Id,
                notification.Hospital,
                notification.Street,
                notification.Number,
                notification.PostalCode,
                notification.City,
                notification.Country)
            {
                AggregateId = notification.Id,
                Type = nameof(HospitalFacilityUpdatedEvent),
                Payload = JsonSerializer.Serialize(notification),
                Version = newVersion
            };

            bool result = await eventStore
                .AddEvent(
                    hospitalFacilityUpdatedEvent,
                    cancellationToken);

            if (!result)
                return;

            HospitalFacility? hospital = await context.Set<HospitalFacility>()
                .FindAsync(notification.Id, cancellationToken);

            hospital!.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Id, cancellationToken));

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
