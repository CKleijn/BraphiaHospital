using Carter;
using MediatR;
using PatientManagement.Common.Annotations;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed class RegisterPatientEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("patient", async (
                ISender sender,
                RegisterPatientCommand command,
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
