using MediatR;

namespace Consultancy.Features.Consult.RegisterConsult.Event
{
    public sealed record ConsultRegisteredEvent(Consult Consult)
        : INotification;
}
