using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.CreateConsult.Command;
using Consultancy.Features.ConsultFeature.CreateConsult.Event;

namespace Consultancy.Common.Interfaces
{
    public interface IConsultMapper
    {
        Consult CreateConsultCommandToConsult(CreateConsultCommand command);
        ConsultCreatedEvent ConsultToConsultCreatedEvent(Consult consult);
    }
}
