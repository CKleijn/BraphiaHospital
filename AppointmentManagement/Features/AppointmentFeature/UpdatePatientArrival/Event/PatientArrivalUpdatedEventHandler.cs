using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event
{
    public sealed class PatientArrivalUpdatedEventHandler(ApplicationDbContext context, IEventStore eventStore)
        : INotificationHandler<PatientArrivalUpdatedEvent>
    {
        public async Task Handle(
            PatientArrivalUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment? appointmentToUpdate = await context.Set<Appointment>()
                .FindAsync(notification.Id, cancellationToken);

            appointmentToUpdate!.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.Id, cancellationToken));

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}