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
            Referral referral = (await context.Set<Referral>().FindAsync(notification.ReferralId, cancellationToken))!;
            var referralEvents = await eventStore.GetAllEventsByAggregateId(referral.Id, referral.Version, cancellationToken);

            if (referralEvents.Any())
                referral.ReplayHistory(referralEvents);

            StaffMember physician = (await context.Set<StaffMember>().FindAsync(notification.PhysicianId, cancellationToken))!;
            var physicianEvents = await eventStore.GetAllEventsByAggregateId(physician.Id, physician.Version, cancellationToken);

            if (physicianEvents.Any())
                physician.ReplayHistory(physicianEvents);

            HospitalFacility hospitalFacility = (await context.Set<HospitalFacility>().FindAsync(notification.HospitalFacilityId, cancellationToken))!;
            var hospitalFacilityEvents = await eventStore.GetAllEventsByAggregateId(hospitalFacility.Id, hospitalFacility.Version, cancellationToken);

            if (hospitalFacilityEvents.Any())
                hospitalFacility.ReplayHistory(hospitalFacilityEvents);

            var appointment = new Appointment
            {
                Id = notification.Id,
                PatientId = notification.PatientId,
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
