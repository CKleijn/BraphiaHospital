using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Command
{
    public sealed record UpdateQuestionCommmand(
        Guid Id,
        string AnswerValue
    ) : IRequest
    {
        public Guid Id { get; set; }
    }
}
