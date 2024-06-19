using Consultancy.Common.Entities;
using Consultancy.Common.Interfaces;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Event;
using Riok.Mapperly.Abstractions;

namespace Consultancy.Common.Mappers
{
    [Mapper]
    public partial class QuestionMapper
        : IQuestionMapper
    {
        public partial QuestionUpdatedEvent QuestionToQuestionUpdatedEvent(Question question);
    }
}
