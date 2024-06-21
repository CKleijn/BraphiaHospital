using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointment.Query
{
    public class GetAppointmentQueryHandler(
        ApplicationDbContext context)
        : IRequestHandler<GetAppointmentQuery, Appointment>
    {
        public async Task<Appointment> Handle(
            GetAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var result = await context.Set<Appointment>()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            return result ?? throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist"); ;
        }
    }
}
