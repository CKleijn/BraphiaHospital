using Carter;
using MediatR;
using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.ReferralFeature.GetReferral.Query;

namespace AppointmentManagement.Features.ReferralFeature.GetReferral
{
    public sealed class GetReferralByReferralIdEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("referral/{id}", async (
                ISender sender,
                Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetReferralByIdQuery(id);

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