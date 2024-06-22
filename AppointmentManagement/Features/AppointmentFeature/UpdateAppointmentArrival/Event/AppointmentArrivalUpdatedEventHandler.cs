using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event
{
    public sealed class AppointmentArrivalUpdatedEventHandler(ApplicationDbContext context, IEventStore eventStore)
        : INotificationHandler<AppointmentArrivalUpdatedEvent>
    {
        public async Task Handle(
            AppointmentArrivalUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment? appointmentToUpdate = await context.Set<Appointment>()
                .FindAsync(notification.Id, cancellationToken);

            var appointmentEvents = await eventStore.GetAllEventsByAggregateId(notification.Id, appointmentToUpdate!.Version, cancellationToken);
            
            if (appointmentEvents.Any())
                appointmentToUpdate!.ReplayHistory(appointmentEvents);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}