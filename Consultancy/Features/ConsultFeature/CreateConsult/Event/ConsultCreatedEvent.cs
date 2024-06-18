using MediatR;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Event
{
    public sealed record ConsultCreatedEvent(Consult Consult)
        : INotification;
}
