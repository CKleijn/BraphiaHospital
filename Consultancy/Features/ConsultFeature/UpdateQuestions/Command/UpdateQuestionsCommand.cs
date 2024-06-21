using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Command
{
    public sealed record UpdateQuestionsCommmand(
        Guid Id,
        List<Question> Questions
    ) : IRequest
    {
        public Guid Id { get; set; }
    }
}
