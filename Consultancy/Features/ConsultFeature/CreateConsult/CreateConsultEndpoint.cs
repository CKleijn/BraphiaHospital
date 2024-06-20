using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.ConsultFeature.CreateConsult.Command;

namespace Consultancy.Features.ConsultFeature.CreateConsult
{
    public sealed class CreateConsultEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("consult", async (
                ISender sender,
                CreateConsultCommand command,
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
            .WithTags(Tags.CONSULT_TAG);
        }
    }
}
