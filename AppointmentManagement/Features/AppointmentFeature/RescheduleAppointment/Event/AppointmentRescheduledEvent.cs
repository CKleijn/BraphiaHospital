using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed record AppointmentRescheduledEvent(Appointment Appointment)
        : INotification;
}
