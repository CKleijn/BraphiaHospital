using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Event
{
    public sealed record DossierConsultAppendedEvent(
            Guid PatientId,
            Consult Consult
        )
        : INotification;
}
