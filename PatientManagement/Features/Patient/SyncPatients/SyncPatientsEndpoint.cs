using Carter;
using MediatR;
using PatientManagement.Common.Annotations;

namespace PatientManagement.Features.Patient.SyncPatients
{
    public sealed class SyncPatientsEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("patients/sync", async (
                ISender sender,
                SyncPatientsCommand command,
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
            .WithTags(Tags.PATIENT_TAG);
        }
    }
}
