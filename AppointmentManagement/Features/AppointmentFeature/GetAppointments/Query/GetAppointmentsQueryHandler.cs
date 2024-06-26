using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointments.Query
{
    public class GetAppointmentsQueryHandler(ApplicationDbContext context, IApiClient apiClient)
        : IRequestHandler<GetAppointmentsQuery, IEnumerable<Appointment>>
    {
        public async Task<IEnumerable<Appointment>> Handle(
            GetAppointmentsQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await context.Set<Appointment>()
                .Include(a => a.Referral)
                .Include(a => a.Physician)
                .Include(a => a.HospitalFacility)
                .ToListAsync(cancellationToken);

            var tasks = appointments.Select(async appointment =>
            {
                var patientUrl = $"{ConfigurationHelper.GetPatientManagementServiceConnectionString()}/patient/{appointment.PatientId}";
                appointment.Patient = (await apiClient.GetAsync<Patient>(patientUrl, cancellationToken))!;
                return appointment;
            });

            await Task.WhenAll(tasks);

            return appointments;
        }
    }
}