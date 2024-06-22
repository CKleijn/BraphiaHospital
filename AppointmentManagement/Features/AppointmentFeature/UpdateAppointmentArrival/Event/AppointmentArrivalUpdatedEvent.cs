using MediatR;
using AppointmentManagement.Common.Enums;
using AppointmentManagement.Common.Abstractions;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event
{
    public sealed class AppointmentArrivalUpdatedEvent(Guid id, ArrivalStatus status)
        : NotificationEvent, INotification
    {
        public Guid Id { get; set; } = id;
        public ArrivalStatus Status { get; set; } = status;
    }
}
