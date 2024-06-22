using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Abstractions;

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
            HospitalFacility? hospital = await context.Set<HospitalFacility>()
                .FindAsync(notification.Id, cancellationToken);

            HospitalFacilityUpdatedEvent hospitalFacilityUpdatedEvent = CorrectPayload(hospital!, notification);

            List<NotificationEvent> events = (await eventStore.GetAllEventsByAggregateId(hospitalFacilityUpdatedEvent.Id, hospital!.Version, cancellationToken)).ToList();

            hospitalFacilityUpdatedEvent.AggregateId = hospitalFacilityUpdatedEvent.Id;
            hospitalFacilityUpdatedEvent.Type = nameof(HospitalFacilityUpdatedEvent);
            hospitalFacilityUpdatedEvent.Payload = JsonSerializer.Serialize(hospitalFacilityUpdatedEvent);
            hospitalFacilityUpdatedEvent.Version = events.Count != 0 ? events.Last().Version + 1 : hospital!.Version + 1;

            bool result = await eventStore
                .AddEvent(
                    hospitalFacilityUpdatedEvent,
                    cancellationToken);

            if (!result)
                return;

            hospital!.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Id, hospital!.Version, cancellationToken));

            await context.SaveChangesAsync(cancellationToken);
        }

        private HospitalFacilityUpdatedEvent CorrectPayload(HospitalFacility prevState, HospitalFacilityUpdatedEvent input)
        {
            if (string.IsNullOrEmpty(input.Hospital))
                input.Hospital = prevState!.Hospital;
            if (string.IsNullOrEmpty(input.Street))
                input.Street = prevState!.Street;
            if (string.IsNullOrEmpty(input.Number))
                input.Number = prevState!.Number;
            if (string.IsNullOrEmpty(input.PostalCode))
                input.PostalCode = prevState!.PostalCode;
            if (string.IsNullOrEmpty(input.City))
                input.City = prevState!.City;
            if (string.IsNullOrEmpty(input.Country))
                input.Country = prevState!.Country;

            return new HospitalFacilityUpdatedEvent(
                prevState.Id,
                input.Hospital,
                input.Street,
                input.Number,
                input.PostalCode,
                input.City,
                input.Country
            );
        }
    }
}
