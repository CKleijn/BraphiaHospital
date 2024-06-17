using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral.Query
{
    public sealed record GetReferralByReferralCodeQuery(string ReferralCode)
        : IRequest<Referral>;
}
