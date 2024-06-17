using Consultancy.Features.Consult.RegisterConsult.Command;
using Consultancy.Features.Consult.RegisterConsult.Event;

namespace Consultancy.Features.Consult._Interfaces
{
    public interface IConsultMapper
    {
        Consult RegisterConsultCommandToConsult(RegisterConsultCommand command);
        ConsultRegisteredEvent ConsultToConsultRegisteredEvent(Consult consult);
    }
}
