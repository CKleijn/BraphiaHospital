using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Features.ConsultFeature.GetConsult.Query
{
    public class GetConsultQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetConsultQuery, Consult>
    {
        public async Task<Consult> Handle(
            GetConsultQuery request,
            CancellationToken cancellationToken)
        {
            var result = await context.Set<Consult>()
                .Include(c => c.Survey)
                .Include(c => c.Survey.Questions)
                .FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            return result == null ? throw new ArgumentNullException($"Consult #{request.id} doesn't exist") : result;
        }
    }
}
