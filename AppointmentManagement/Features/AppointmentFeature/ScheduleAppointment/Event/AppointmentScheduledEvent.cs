using MediatR;
using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Enums;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed class AppointmentScheduledEvent(
        Guid id,
        Guid patientId,
        Guid referralId,
        Guid physicianId,
        Guid hospitalFacilityId,
        DateTime scheduledDateTime,
        ArrivalStatus status)
        : NotificationEvent, INotification
    {
        public Guid Id { get; set; } = id;
        public Guid PatientId { get; set; } = patientId;
        public Guid ReferralId { get; set; } = referralId;
        public Guid PhysicianId { get; set; } = physicianId;
        public Guid HospitalFacilityId { get; set; } = hospitalFacilityId;
        public DateTime ScheduledDateTime { get; set; } = scheduledDateTime;
        public ArrivalStatus Status { get; set; } = status;
    }
}
