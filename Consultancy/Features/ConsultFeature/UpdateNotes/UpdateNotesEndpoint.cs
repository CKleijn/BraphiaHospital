using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.ConsultFeature.UpdateNotes.Command;

namespace Consultancy.Features.ConsultFeature.UpdateNotes
{
    public sealed class UpdateNotesEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("consult/{id}", async (
                ISender sender,
                Guid id,
                UpdateNotesCommand command,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    command.Id = id;
                    await sender.Send(command, cancellationToken);

                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            })
            .WithTags(Tags.CONSULT_TAG);
        }
    }
}
