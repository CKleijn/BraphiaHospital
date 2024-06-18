﻿using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{
    public sealed class AppointmentRescheduledEventHandler(ApplicationDbContext context)
        : INotificationHandler<AppointmentRescheduledEvent>
    {
        public async Task Handle(
            AppointmentRescheduledEvent notification,
            CancellationToken cancellationToken)
        {
            Appointment? appointmentToUpdate = await context.Set<Appointment>()
                .FindAsync(notification.Appointment.Id, cancellationToken);

            if (appointmentToUpdate == null)
                throw new ArgumentNullException($"Appointment #{notification.Appointment.Id} doesn't exist");

            appointmentToUpdate.ScheduledDateTime = notification.Appointment.ScheduledDateTime;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}