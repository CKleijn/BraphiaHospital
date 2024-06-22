using Consultancy.Common.Abstractions;
using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent
{
    public sealed class DossierConsultAppendedEvent(
            Consult Consult
        )
        : Event, INotification
    {
        public Consult Consult { get; set; } = Consult;
    }
}
