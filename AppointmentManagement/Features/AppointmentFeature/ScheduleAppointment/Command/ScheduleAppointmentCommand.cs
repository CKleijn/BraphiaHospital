using AppointmentManagement.Common.Entities;
using AppointmentManagement.Common.Enums;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed record ScheduleAppointmentCommand(
        Guid PatientId,
        Guid ReferralId,
        Guid PhysicianId,
        Guid HospitalFacilityId,
        ArrivalStatus Status,
        DateTime ScheduledDateTime
    ) : IRequest;
}
