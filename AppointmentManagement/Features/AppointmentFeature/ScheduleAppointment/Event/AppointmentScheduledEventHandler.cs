using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using Azure.Core;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{ 
    public sealed class AppointmentScheduledEventHandler(ApplicationDbContext context)
        : INotificationHandler<AppointmentScheduledEvent>
    {
        public async Task Handle(
            AppointmentScheduledEvent notification,
            CancellationToken cancellationToken)
        {

            Appointment appointment = new()
            {
                Id = notification.Id,
                PatientId = notification.PatientId,
                ReferralId = notification.ReferralId,
                PhysicianId = notification.PhysicianId,
                HospitalFacilityId = notification.HospitalFacilityId,
                ScheduledDateTime = notification.ScheduledDateTime
            };

            context.Set<Appointment>().Add(appointment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
