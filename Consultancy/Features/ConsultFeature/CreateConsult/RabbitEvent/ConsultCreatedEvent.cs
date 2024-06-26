using Consultancy.Common.Abstractions;
using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent
{
    public sealed class ConsultCreatedEvent(
            Consult Consult
        )
        : Event, INotification
    {
        public Consult Consult { get; set; } = Consult;
    }
}
