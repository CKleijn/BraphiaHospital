using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointments.Query
{
    public class GetAppointmentsQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetAppointmentsQuery, IEnumerable<Appointment>>
    {
        public async Task<IEnumerable<Appointment>> Handle(
            GetAppointmentsQuery request,
            CancellationToken cancellationToken)
        {

            return await context.Set<Appointment>()
                .Include(a => a.Patient)
                .Include(a => a.Referral)
                .Include(a => a.Physician)
                .Include(a => a.HospitalFacility)
                .ToListAsync(cancellationToken);
        }
    }
}
