using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using System.Text.Json;

namespace AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event
{
    public sealed class HospitalFacilityCreatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<HospitalFacilityCreatedEvent>
    {
        public async Task Handle(
            HospitalFacilityCreatedEvent notification,
            CancellationToken cancellationToken)
        {

            HospitalFacilityCreatedEvent hospitalFacilityCreatedEvent = new(notification.HospitalFacility)
            {
                AggregateId = notification.HospitalFacility.Id,
                Type = nameof(HospitalFacilityCreatedEvent),
                Payload = JsonSerializer.Serialize(notification.HospitalFacility),
            };

            bool result = await eventStore
                .AddEvent(
                    hospitalFacilityCreatedEvent,
                    cancellationToken);

            if (!result)
                return;

            context.Set<HospitalFacility>().Add(notification.HospitalFacility);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}