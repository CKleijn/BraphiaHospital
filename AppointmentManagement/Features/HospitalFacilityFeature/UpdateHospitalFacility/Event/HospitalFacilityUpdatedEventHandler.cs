using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;

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

            HospitalFacility? hospitalToUpdate = await context.Set<HospitalFacility>()
                .FindAsync(notification.HospitalFacility.Id, cancellationToken);

            var result = await eventStore
                .AddEvent(
                    typeof(HospitalFacilityUpdatedEvent).Name,
                    JsonSerializer.Serialize(notification.HospitalFacility),
                    cancellationToken);

            if (!result)
                return;

            // update through event sourcing
            hospitalToUpdate = notification.HospitalFacility;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}