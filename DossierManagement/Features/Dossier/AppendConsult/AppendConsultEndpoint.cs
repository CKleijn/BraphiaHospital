using Carter;
using DossierManagement.Common.Annotations;
using MediatR;

namespace DossierManagement.Features.Dossier.AppendConsult
{
    public sealed class AppendConsultEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("dossier/consult", async (
                ISender sender,
                AppendConsultCommand command,
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
