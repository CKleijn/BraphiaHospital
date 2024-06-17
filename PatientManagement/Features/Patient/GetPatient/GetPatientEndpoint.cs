using Carter;
using MediatR;
using PatientManagement.Common.Annotations;
using PatientManagement.Features.Patient.GetPatient.Query;

namespace PatientManagement.Features.Patient.GetPatient
{
    public sealed class GetPatientEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("patient/{id}", async (
                ISender sender,
                Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetPatientQuery(id);

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
