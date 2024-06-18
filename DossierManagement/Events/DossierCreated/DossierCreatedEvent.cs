using MediatR;

namespace DossierManagement.Events.DossierCreated
{
    public sealed record DossierCreatedEvent(
        Guid Id,
        Guid PatientId)
        : INotification;
}
