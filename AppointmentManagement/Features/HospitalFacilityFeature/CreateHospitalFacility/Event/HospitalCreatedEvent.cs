using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event
{
    public sealed class HospitalFacilityCreatedEvent(HospitalFacility hospitalFacility)
        : NotificationEvent, INotification
    {
        public HospitalFacility HospitalFacility { get; set; } = hospitalFacility;
    }
}