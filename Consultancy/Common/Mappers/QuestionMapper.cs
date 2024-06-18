using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature._Interfaces;
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
