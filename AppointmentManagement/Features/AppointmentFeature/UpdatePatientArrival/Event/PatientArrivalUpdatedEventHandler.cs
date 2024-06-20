using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event
{
    public sealed class PatientArrivalUpdatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<PatientArrivalUpdatedEvent>
    {
        public async Task Handle(
            PatientArrivalUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment? appointmentToUpdate = await context.Set<Appointment>()
                .FindAsync(notification.Appointment.Id, cancellationToken);

            if (appointmentToUpdate == null)
                throw new ArgumentNullException($"Appointment #{notification.Appointment.Id} doesn't exist");

            appointmentToUpdate.Status = notification.Appointment.Status;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}