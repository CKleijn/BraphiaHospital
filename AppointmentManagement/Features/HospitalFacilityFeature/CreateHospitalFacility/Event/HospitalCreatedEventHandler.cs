using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
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
            var result = await eventStore
                .AddEvent(
                    typeof(HospitalFacilityCreatedEvent).Name,
                    JsonSerializer.Serialize(notification.HospitalFacility),
                    cancellationToken);

            if (!result)
                return;

            context.Set<HospitalFacility>().Add(notification.HospitalFacility);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}