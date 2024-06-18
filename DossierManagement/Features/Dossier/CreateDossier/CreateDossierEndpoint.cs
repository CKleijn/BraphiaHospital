using Carter;
using DossierManagement.Common.Annotations;
using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier
{
    public sealed class CreateDossierEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("dossier", async (
                ISender sender,
                CreateDossierCommand command,
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
            .WithTags(Tags.DOSSIER_TAG);
        }
    }
}
