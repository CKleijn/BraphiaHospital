using AppointmentManagement.Common.Entities;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using Azure.Core;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event
{ 
    public sealed class AppointmentScheduledEventHandler(ApplicationDbContext context, IApiClient apiClient)
        : INotificationHandler<AppointmentScheduledEvent>
    {
        public async Task Handle(
            AppointmentScheduledEvent notification,
            CancellationToken cancellationToken)
        {
            _ = await apiClient
                .GetAsync<Patient>($"{ConfigurationHelper.GetPatientManagementServiceConnectionString()}/patient/{notification.PatientId}", cancellationToken)
                ?? throw new ArgumentNullException($"Patient #{notification.PatientId} doesn't exist");
            _ = await context.Set<Referral>()
            .FindAsync(notification.ReferralId, cancellationToken) ?? throw new ArgumentNullException($"Referral #{notification.ReferralId} doesn't exist");
            _ = await context.Set<StaffMember>()
            .FindAsync(notification.PhysicianId, cancellationToken) ?? throw new ArgumentNullException($"Physician #{notification.PhysicianId} doesn't exist");
            _ = await context.Set<HospitalFacility>()
            .FindAsync(notification.HospitalFacilityId, cancellationToken) ?? throw new ArgumentNullException($"HospitalFacility #{notification.HospitalFacilityId} doesn't exist");

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
