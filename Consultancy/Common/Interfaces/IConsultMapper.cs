using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.UpdateQuestions.Event;

namespace Consultancy.Common.Interfaces
{
    public interface IConsultMapper
    {
        ConsultSurveyFilledInEvent ConsultToConsultSurveyFilledInEvent(Consult Consult);
    }
}
