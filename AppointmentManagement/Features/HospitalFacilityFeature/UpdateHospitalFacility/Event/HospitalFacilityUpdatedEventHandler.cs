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

            var result = await eventStore
                .AddEvent(
                    typeof(HospitalFacilityUpdatedEvent).Name,
                    JsonSerializer.Serialize(notification.HospitalFacility),
                    cancellationToken);

            if (!result)
                return;

            HospitalFacility? hospitalToUpdate = await context.Set<HospitalFacility>()
                .FindAsync(notification.HospitalFacility.Id, cancellationToken);

            //hospital might not exist within appointment management context

            if (hospitalToUpdate == null)
                return;

            // update through event sourcing

            hospitalToUpdate!.Street = notification.HospitalFacility.Street;
            hospitalToUpdate!.Number = notification.HospitalFacility.Number;
            hospitalToUpdate!.PostalCode = notification.HospitalFacility.PostalCode;
            hospitalToUpdate!.City = notification.HospitalFacility.City;
            hospitalToUpdate!.Country = notification.HospitalFacility.Country;
            hospitalToUpdate!.Stores = notification.HospitalFacility.Stores;
            hospitalToUpdate!.Squares = notification.HospitalFacility.Squares;
            hospitalToUpdate!.PhoneNumber = notification.HospitalFacility.PhoneNumber;
            hospitalToUpdate!.Email = notification.HospitalFacility.Email;
            hospitalToUpdate!.Website = notification.HospitalFacility.Website;
            hospitalToUpdate!.TotalBeds = notification.HospitalFacility.TotalBeds;
            hospitalToUpdate!.BuiltYear = notification.HospitalFacility.BuiltYear;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
