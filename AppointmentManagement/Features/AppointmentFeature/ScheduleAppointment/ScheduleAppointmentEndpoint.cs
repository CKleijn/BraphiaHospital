using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment
{
    public sealed class ScheduleAppointmentEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("appointment", async (
                ISender sender,
                ScheduleAppointmentCommand command,
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
