using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.ConsultFeature.UpdateQuestions.Command;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions
{
    public sealed class UpdateQuestionsEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("consult/{id}/survey", async (
                ISender sender,
                Guid id,
                UpdateQuestionsCommmand command,
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
