using Carter;
using MediatR;
using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.GetAppointments.Query;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointments
{
    public sealed class GetAppointmentsEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("appointment", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetAppointmentsQuery();

                    var result = await sender.Send(query, cancellationToken);

                    return Results.Ok(result);
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