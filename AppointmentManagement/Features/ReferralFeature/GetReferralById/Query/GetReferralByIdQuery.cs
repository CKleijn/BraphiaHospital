using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.ReferralFeature.GetReferralById.Query
{
    public sealed record GetReferralByIdQuery(Guid Id)
        : IRequest<Referral>;
}
