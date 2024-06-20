using Consultancy.Common.Entities;
using Consultancy.Common.Interfaces;
using Consultancy.Features.ConsultFeature.UpdateQuestions.Event;
using Riok.Mapperly.Abstractions;

namespace Consultancy.Common.Mappers
{
    [Mapper]
    public partial class ConsultMapper
        : IConsultMapper
    {
        public partial SurveyFilledInEvent ConsultToSurveyFilledInEvent(Consult Consult);
    }
}
