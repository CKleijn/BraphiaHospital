using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral.Query
{
    public sealed record GetReferralByIdQuery(Guid Id)
        : IRequest<Referral>;
}
