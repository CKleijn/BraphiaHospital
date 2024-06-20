using MediatR;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Event
{
    public sealed record SurveyFilledInEvent(Consult Consult)
        : INotification;
}
