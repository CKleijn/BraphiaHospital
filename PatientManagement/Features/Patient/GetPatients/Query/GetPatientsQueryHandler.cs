using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Infrastructure.Persistence.Contexts;

namespace PatientManagement.Features.Patient.GetPatients.Query
{
    public sealed class GetPatientsQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetPatientsQuery, IEnumerable<Patient>>
    {
        public async Task<IEnumerable<Patient>> Handle(
            GetPatientsQuery request, 
            CancellationToken cancellationToken)
        {
            return await context.Set<Patient>().ToListAsync(cancellationToken);
        }
    }
}
