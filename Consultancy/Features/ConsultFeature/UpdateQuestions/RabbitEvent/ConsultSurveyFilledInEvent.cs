using MediatR;
using Consultancy.Common.Entities;
using Consultancy.Common.Abstractions;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent
{
    public sealed class ConsultSurveyFilledInEvent(
            ICollection<Question> Questions
        )
        : Event, INotification
    {
        public ICollection<Question> Questions { get; set; } = Questions;
    }
}
