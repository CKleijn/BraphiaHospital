using Carter;
using DossierManagement.Common.Annotations;
using MediatR;

namespace DossierManagement.Features.Dossier.GetDossierById
{
    public sealed class GetDossierByPatientEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("dossier/{id}", async (
                ISender sender,
                Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetDossierByIdQuery(id);

                    var result = await sender.Send(query, cancellationToken);

                    return Results.Ok(result);
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
