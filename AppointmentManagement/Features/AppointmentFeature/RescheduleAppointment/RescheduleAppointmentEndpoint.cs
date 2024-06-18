using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command;
using Carter;
using MediatR;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral
{
    public sealed class ScheduleAppointmentEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("appointment/reschedule/{id}", async (
                ISender sender,
                Guid id,
                RescheduleAppointmentCommand command,
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
