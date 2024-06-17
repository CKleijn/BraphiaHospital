using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.Consult.RegisterConsult.Command;

namespace Consultancy.Features.Consult.RegisterConsult
{
    public sealed class RegisterConsultEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("consult", async (
                ISender sender,
                RegisterConsultCommand command,
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
