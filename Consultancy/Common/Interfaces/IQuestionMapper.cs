using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Event;

namespace Consultancy.Common.Interfaces
{
    public interface IQuestionMapper
    {
        QuestionUpdatedEvent QuestionToQuestionUpdatedEvent(Question question);
    }
}
