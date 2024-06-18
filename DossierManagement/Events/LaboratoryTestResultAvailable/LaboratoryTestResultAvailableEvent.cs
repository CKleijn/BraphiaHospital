using DossierManagement.Features.Dossier;
using MediatR;

namespace DossierManagement.Events.LabTestResultAvailable
{
    public sealed record LaboratoryTestResultAvailableEvent(
        Guid PatientId, 
        Result Result)
        : INotification;
}
