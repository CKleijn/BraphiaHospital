using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Infrastructure.Persistence.Contexts;

namespace PatientManagement.Features.Patient.GetPatientById
{
    public class GetPatientByIdQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetPatientByIdQuery, Patient>
    {
        public async Task<Patient> Handle(
            GetPatientByIdQuery request,
            CancellationToken cancellationToken)
        {
            var patient = await context
                .Set<Patient>()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new ArgumentNullException($"Patient #{request.Id} doesn't exist");

            return patient;
        }
    }
}
