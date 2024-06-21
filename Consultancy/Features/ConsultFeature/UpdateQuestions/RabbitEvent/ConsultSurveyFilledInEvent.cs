using MediatR;
using Consultancy.Common.Entities;
using Consultancy.Common.Abstractions;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent
{
    public sealed class ConsultSurveyFilledInEvent(
            Consult Consult
        )
        : Event, INotification
    {
        public Consult Consult { get; set; } = Consult;
    }
}
