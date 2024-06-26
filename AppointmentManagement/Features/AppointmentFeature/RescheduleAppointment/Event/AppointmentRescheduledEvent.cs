using MediatR;
using AppointmentManagement.Common.Abstractions;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed class AppointmentRescheduledEvent(Guid id, DateTime scheduledDateTime)
        : NotificationEvent, INotification
    {
        public Guid Id { get; set; } = id;
        public DateTime ScheduledDateTime { get; set; } = scheduledDateTime;
    }
}
