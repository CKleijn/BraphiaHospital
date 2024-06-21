using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event
{
    public sealed class HospitalFacilityUpdatedEvent(Guid id, string hospital, string street, string number, string postalCode, string city, string country)
        : NotificationEvent, INotification
    {
        public Guid Id { get; set; } = id;
        public string Hospital { get; set; } = hospital;
        public string Street { get; set; } = street;
        public string Number { get; set; } = number;
        public string PostalCode { get; set; } = postalCode;
        public string City { get; set; } = city;
        public string Country { get; set; } = country;
    }
}