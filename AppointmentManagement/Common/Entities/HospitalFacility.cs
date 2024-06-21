using AppointmentManagement.Common.Aggregates;
using AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event;

namespace AppointmentManagement.Common.Entities
{
    public class HospitalFacility
        : AggregateRoot
    {
        public string Hospital { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public void Apply(HospitalFacilityCreatedEvent @event)
        {
            Hospital = @event.HospitalFacility.Hospital;
            Street = @event.HospitalFacility.Street;
            Number = @event.HospitalFacility.Number;
            PostalCode = @event.HospitalFacility.PostalCode;
            City = @event.HospitalFacility.City;
            Country = @event.HospitalFacility.Country;
        }

        public void Apply(HospitalFacilityUpdatedEvent @event)
        {
            Hospital = @event.Hospital;
            Street = @event.Street;
            Number = @event.Number;
            PostalCode = @event.PostalCode;
            City = @event.City;
            Country = @event.Country;
        }
    }
}