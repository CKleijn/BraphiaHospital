using AppointmentManagement.Common.Annotations;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.Referral.CreateReferral
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
            .WithTags(Tags.APPOINTMENT_TAG);
        }
    }
}
