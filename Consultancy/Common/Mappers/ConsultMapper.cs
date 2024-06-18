using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature._Interfaces;
using Consultancy.Features.ConsultFeature.CreateConsult.Command;
using Consultancy.Features.ConsultFeature.CreateConsult.Event;
using Riok.Mapperly.Abstractions;

namespace Consultancy.Common.Mappers
{
    [Mapper]
    public partial class ConsultMapper
        : IConsultMapper
    {
        public partial Consult CreateConsultCommandToConsult(CreateConsultCommand command);
        public partial ConsultCreatedEvent ConsultToConsultCreatedEvent(Consult consult);
    }
}
