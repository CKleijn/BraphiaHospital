using Carter;
using DossierManagement.Common.Annotations;
using MediatR;

namespace DossierManagement.Features.Dossier.PrescribeMedication
{
    public sealed class PrescribeMedicationEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("dossier/medication", async (
                ISender sender,
                PrescribeMedicationCommand command,
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
