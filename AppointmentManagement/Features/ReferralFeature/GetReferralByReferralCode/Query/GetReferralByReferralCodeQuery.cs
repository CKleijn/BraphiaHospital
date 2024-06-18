using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferralByReferralCode.Query
{
    public sealed record GetReferralByReferralCodeQuery(string ReferralCode)
        : IRequest<Referral>;
}
