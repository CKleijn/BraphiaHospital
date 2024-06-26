using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed class AppointmentRescheduledEventHandler(ApplicationDbContext context, IEventStore eventStore)
        : INotificationHandler<AppointmentRescheduledEvent>
    {
        public async Task Handle(
            AppointmentRescheduledEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment? appointmentToUpdate = await context.Set<Appointment>()
                .FindAsync(notification.Id, cancellationToken);

            var appointmentEvents = await eventStore.GetAllEventsByAggregateId(notification.Id, appointmentToUpdate!.Version, cancellationToken);

            if(appointmentEvents.Any())
                appointmentToUpdate!.ReplayHistory(appointmentEvents);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}