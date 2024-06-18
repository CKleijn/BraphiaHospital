using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed record ScheduleAppointmentCommand(
        Guid PatientId,
        Guid ReferralId,
        Guid PhysicianId,
        Guid HospitalFacilityId,
        DateTime ScheduledDateTime
    ) : IRequest;
}
