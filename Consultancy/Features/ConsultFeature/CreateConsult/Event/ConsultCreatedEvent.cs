using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Event
{
    public sealed record ConsultCreatedEvent(
            Guid Id,
            Guid AppointmentId,
            Guid PatientId,
            Survey? Survey
        )
        : INotification;
}
