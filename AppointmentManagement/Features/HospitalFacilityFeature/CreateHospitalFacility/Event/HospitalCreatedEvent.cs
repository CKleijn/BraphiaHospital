using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event
{
    public sealed record HospitalFacilityCreatedEvent(HospitalFacility HospitalFacility)
        : INotification;
}