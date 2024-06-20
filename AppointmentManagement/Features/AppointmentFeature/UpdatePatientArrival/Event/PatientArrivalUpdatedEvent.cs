using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event
{
    public sealed record PatientArrivalUpdatedEvent(Appointment Appointment)
        : INotification;
}
