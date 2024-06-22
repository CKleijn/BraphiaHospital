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
            Referral referral = (await context.Set<Referral>().FindAsync(notification.ReferralId, cancellationToken))!;
            referral.ReplayHistory(referralEvents);

            var physicianEvents = await eventStore.GetAllEventsByAggregateId(notification.PhysicianId, cancellationToken);
            StaffMember physician = (await context.Set<StaffMember>().FindAsync(notification.PhysicianId, cancellationToken))!;

            physician.ReplayHistory(physicianEvents);

            var hospitalFacilityEvents = await eventStore.GetAllEventsByAggregateId(notification.HospitalFacilityId, cancellationToken);
            HospitalFacility hospitalFacility = (await context.Set<HospitalFacility>().FindAsync(notification.HospitalFacilityId, cancellationToken))!;
            hospitalFacility.ReplayHistory(hospitalFacilityEvents);

            //fix patient when patient api is available
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

            context.Set<Appointment>().Add(appointment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
