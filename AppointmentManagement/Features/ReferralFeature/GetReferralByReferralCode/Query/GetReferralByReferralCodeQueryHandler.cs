using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferralByReferralCode.Query
{
    public class GetReferralByReferralCodeQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetReferralByReferralCodeQuery, Referral>
    {
        public async Task<Referral> Handle(
            GetReferralByReferralCodeQuery request,
            CancellationToken cancellationToken)
        {
            var result = await context.Set<Referral>().FirstOrDefaultAsync(p => p.ReferralCode == request.ReferralCode, cancellationToken);

            return result == null ? throw new ArgumentNullException($"Referral ({request.ReferralCode}) doesn't exist") : result;
        }
    }
}
