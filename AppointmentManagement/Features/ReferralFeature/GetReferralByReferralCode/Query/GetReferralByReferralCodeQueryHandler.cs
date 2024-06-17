using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral.Query
{
    public class GetReferralQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetReferralByReferralCodeQuery, Referral>
    {
        public async Task<Referral> Handle(
            GetReferralByReferralCodeQuery request,
            CancellationToken cancellationToken)
        {
            var referral = await context.Set<Referral>().FirstOrDefaultAsync(p => p.ReferralCode == request.ReferralCode, cancellationToken);

            if (referral == null)
                throw new ArgumentNullException($"Referral ({request.ReferralCode}) doesn't exist");

            return referral;
        }
    }
}
