using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Infrastructure.Persistence.Contexts;

namespace PatientManagement.Features.Patient.GetPatient.Query
{
    public class GetPatientQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetPatientQuery, Patient>
    {
        public async Task<Patient> Handle(
            GetPatientQuery request, 
            CancellationToken cancellationToken)
        {
            var patient = await context.Set<Patient>().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (patient == null) 
                throw new ArgumentNullException($"Patient #{request.Id} doesn't exist");

            return patient;
        }
    }
}
