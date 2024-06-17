using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral.Query
{
    public class GetReferralByIdQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetReferralByIdQuery, Referral>
    {
        public async Task<Referral> Handle(
            GetReferralByIdQuery request,
            CancellationToken cancellationToken)
        {
            var referral = await context.Set<Referral>().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (referral == null)
                throw new ArgumentNullException($"Referral #{request.Id} doesn't exist");

            return referral;
        }
    }
}
