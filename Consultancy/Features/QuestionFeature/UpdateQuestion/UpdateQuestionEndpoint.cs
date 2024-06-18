using Carter;
using MediatR;
using Consultancy.Common.Annotations;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Command;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion
{
    public sealed class UpdateQuestionEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("question/{id}", async (
                ISender sender,
                Guid id,
                UpdateQuestionCommmand command,
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
            .WithTags(Tags.QUESTION_TAG);
        }
    }
}
