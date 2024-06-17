using Carter;
using MediatR;
using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Features.AppointmentFeature.GetAppointment.Query;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointment
{
    public sealed class GetAppointmentEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("appointment/{id}", async (
                ISender sender,
                Guid id,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var query = new GetAppointmentQuery(id);

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