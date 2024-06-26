using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Features.ConsultFeature.GetConsults.Query
{
    public class GetConsultsQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetConsultsQuery, IEnumerable<Consult>>
    {
        public async Task<IEnumerable<Consult>> Handle(
            GetConsultsQuery request,
            CancellationToken cancellationToken)
        {
            var consults = await context.Set<Consult>()
                .Include(c => c.Survey)
                .ToListAsync(cancellationToken);

            foreach (var consult in consults)
            {
                if (consult.Survey != null)
                {
                    await context.Entry(consult.Survey)
                        .Collection(s => s.Questions)
                        .LoadAsync(cancellationToken);
                }
            }

            return consults ?? [];
        }
    }
}
