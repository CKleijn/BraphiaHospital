using DossierManagement.Common.Abstractions;
using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed class DossierConsultAppendedEvent(Consult consult)
       : Event, INotification
    {
        public Consult Consult { get; set; } = consult;
    }
}
