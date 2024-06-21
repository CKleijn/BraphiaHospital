using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{ 
    public sealed class AppointmentScheduledEventHandler(ApplicationDbContext context, IEventStore eventStore)
        : INotificationHandler<AppointmentScheduledEvent>
    {
        public async Task Handle(
            AppointmentScheduledEvent notification,
            CancellationToken cancellationToken)
        {
            var referralEvents = await eventStore.GetAllEventsByAggregateId(notification.ReferralId, cancellationToken);
            Referral referral = new() { Id = notification.ReferralId };
            referral.ReplayHistory(referralEvents);

            var physicianEvents = await eventStore.GetAllEventsByAggregateId(notification.PhysicianId, cancellationToken);
            StaffMember physician = new() { Id = notification.PhysicianId };
            physician.ReplayHistory(physicianEvents);

            var hospitalFacilityEvents = await eventStore.GetAllEventsByAggregateId(notification.HospitalFacilityId, cancellationToken);
            HospitalFacility hospitalFacility = new() { Id = notification.HospitalFacilityId };
            hospitalFacility.ReplayHistory(hospitalFacilityEvents);

            var patient = new Patient();

            var appointment = new Appointment
            {
                Id = notification.Id,
                Patient = patient,
                Referral = referral,
                Physician = physician,
                HospitalFacility = hospitalFacility,
                ScheduledDateTime = notification.ScheduledDateTime,
                Status = notification.Status
            };

            //context.Set<Appointment>().Add(appointment);
           // await context.SaveChangesAsync(cancellationToken);
        }
    }
}
