using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Event;

namespace Consultancy.Features.ConsultFeature._Interfaces
{
    public interface IQuestionMapper
    {
        QuestionUpdatedEvent QuestionToQuestionUpdatedEvent(Question question);
    }
}
