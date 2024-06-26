using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.ConsultFeature.GetConsults.Query;

namespace Consultancy.Features.ConsultFeature.GetConsults
{
    public sealed class GetConsultsEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("consult", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetConsultsQuery();

                    var result = await sender.Send(query, cancellationToken);

                    return Results.Ok(result);
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
