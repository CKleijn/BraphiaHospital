using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using Azure.Core;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{ 
    public sealed class AppointmentScheduledEventHandler(ApplicationDbContext context, IEventStore eventStore, IApiClient apiClient)
        : INotificationHandler<AppointmentScheduledEvent>
    {
        public async Task Handle(
            AppointmentScheduledEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment appointment = new();
            appointment.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.AggregateId, null, cancellationToken));

            context.Set<Appointment>().Add(appointment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
