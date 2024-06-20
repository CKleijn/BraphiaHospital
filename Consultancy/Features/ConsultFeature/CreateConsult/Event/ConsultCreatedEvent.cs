using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Event
{
    public sealed record ConsultCreatedEvent(
            Guid PatientId,
            Consult Consult
        )
        : INotification;
}
