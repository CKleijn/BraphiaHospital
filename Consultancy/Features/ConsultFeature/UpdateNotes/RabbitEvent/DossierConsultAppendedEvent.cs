using Consultancy.Common.Abstractions;
using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent
{
    public sealed class DossierConsultAppendedEvent(
            string Notes
        )
        : Event, INotification
    {
        public string Notes { get; set; } = Notes;
    }
}
