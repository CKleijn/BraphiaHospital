using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed record AppointmentScheduledEvent(Appointment Appointment)
        : INotification;
}
