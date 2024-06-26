using Carter;
using MediatR;
using PatientManagement.Common.Annotations;

namespace PatientManagement.Features.Patient.GetPatients
{
    public sealed class GetPatientsEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("patients", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetPatientsQuery();

                    var result = await sender.Send(query, cancellationToken);

                    return Results.Ok(result);
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(Tags.PATIENT_TAG);
        }
    }
}
