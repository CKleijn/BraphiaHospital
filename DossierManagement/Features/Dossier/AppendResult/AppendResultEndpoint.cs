using Carter;
using DossierManagement.Common.Annotations;
using MediatR;

namespace DossierManagement.Features.Dossier.AppendResult
{
    public sealed class AppendResultEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("dossier/result", async (
                ISender sender,
                AppendResultCommand command,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    await sender.Send(command, cancellationToken);

                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(Tags.DOSSIER_TAG);
        }
    }
}
