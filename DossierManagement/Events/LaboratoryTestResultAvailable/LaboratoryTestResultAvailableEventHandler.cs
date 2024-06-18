using DossierManagement.Features.Dossier.AppendResult;
using MediatR;

namespace DossierManagement.Events.LabTestResultAvailable
{
    public sealed class LaboratoryTestResultAvailableEventHandler(ISender sender)
        : INotificationHandler<LaboratoryTestResultAvailableEvent>
    {
        public async Task Handle(
            LaboratoryTestResultAvailableEvent notification,
            CancellationToken cancellationToken)
        {
            var command = new AppendResultCommand(notification.PatientId, notification.Result);
            await sender.Send(command, cancellationToken);
        }
    }
}
