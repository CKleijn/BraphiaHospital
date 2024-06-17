using Consultancy.Features.Consult._Interfaces;
using Consultancy.Features.Consult.RegisterConsult.Command;
using Consultancy.Features.Consult.RegisterConsult.Event;
using Riok.Mapperly.Abstractions;

namespace Consultancy.Features.Consult
{
    [Mapper]
    public partial class ConsultMapper
        : IConsultMapper
    {
        public partial Consult RegisterConsultCommandToConsult(RegisterConsultCommand command);
        public partial ConsultRegisteredEvent ConsultToConsultRegisteredEvent(Consult consult);
    }
}
