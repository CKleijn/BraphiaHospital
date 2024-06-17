using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Command;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral
{
    public sealed class CreateReferralEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("referral", async (
                ISender sender,
                CreateReferralCommand command,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    await sender.Send(command, cancellationToken);

                    return Results.Created();
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
