using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival
{
    public sealed class UpdatePatientArrivalEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("appointment/arrival/{id}", async (
                ISender sender,
                UpdatePatientArrivalCommand command,
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
            .WithTags(Tags.APPOINTMENT_TAG);
        }
    }
}
