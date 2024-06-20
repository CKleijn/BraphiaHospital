using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event
{
    public sealed record HospitalFacilityUpdatedEvent(HospitalFacility HospitalFacility)
        : INotification;
}


