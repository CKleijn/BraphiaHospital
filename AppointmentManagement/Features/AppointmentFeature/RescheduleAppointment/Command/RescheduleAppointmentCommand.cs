using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command
{
    public sealed record RescheduleAppointmentCommand(
        Guid Id,
        DateTime ScheduledDateTime
    ) : IRequest
    {
        public Guid Id { get; set; }
    }
}
