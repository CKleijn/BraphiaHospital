using Carter;
using MediatR;
using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.ReferralFeature.GetReferral.Query;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral
{
    public sealed class GetReferralByReferralCodeEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("referral/code/{referralCode}", async (
                ISender sender,
                string referralCode,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetReferralByReferralCodeQuery(referralCode);

                    var result = await sender.Send(query, cancellationToken);

                    return Results.Ok(result);
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(Tags.REFERRAL_TAG);
        }
    }
}