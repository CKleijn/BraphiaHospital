using MediatR;
using AppointmentManagement.Common.Enums;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed record AppointmentScheduledEvent(
            Guid Id,
            Guid PatientId,
            Guid ReferralId,
            Guid PhysicianId,
            Guid HospitalFacilityId,
            ArrivalStatus Status,
            DateTime ScheduledDateTime
        )
        : INotification;
}
