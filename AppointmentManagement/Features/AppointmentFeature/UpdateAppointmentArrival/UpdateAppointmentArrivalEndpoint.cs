using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival
{
    public sealed class UpdateAppointmentArrivalEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("appointment/arrival/{id}", async (
                ISender sender,
                Guid id,
                UpdateAppointmentArrivalCommand command,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    command.Id = id;
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
