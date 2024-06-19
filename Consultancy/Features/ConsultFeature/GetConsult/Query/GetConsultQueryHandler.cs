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
            var consult = await context.Set<Consult>()
                .Include(c => c.Survey)
                .FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            if (consult?.Survey != null)
            {
                await context.Entry(consult.Survey)
                    .Collection(s => s.Questions)
                    .LoadAsync(cancellationToken);
            }

            return consult ?? throw new ArgumentNullException($"Consult #{request.id} doesn't exist");
        }
    }
}
