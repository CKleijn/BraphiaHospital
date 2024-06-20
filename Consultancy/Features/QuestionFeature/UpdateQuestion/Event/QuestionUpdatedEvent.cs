using MediatR;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Event
{
    public sealed record QuestionUpdatedEvent(Question Question)
        : INotification;
}
