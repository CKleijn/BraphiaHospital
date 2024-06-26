using DossierManagement.Common.Helpers;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.PrescribeMedication
{
    public sealed class PrescribeMedicationCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        IValidator<PrescribeMedicationCommand> validator)
        : IRequestHandler<PrescribeMedicationCommand>
    {
        public async Task Handle(
            PrescribeMedicationCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var dossierAggregateId = await eventStore.GetDossierAggregateIdByPatientId(request.PatientId, cancellationToken);

            if (dossierAggregateId == Guid.Empty)
                throw new ArgumentNullException($"Dossier with patient #{request.PatientId} doesn't exists");

            var latestVersion = await eventStore.GetLatestVersionOfEventByAggregateId(dossierAggregateId, nameof(DossierMedicationPrescribedEvent), cancellationToken);
            latestVersion++;

            var medicationPrescribedEvent = new DossierMedicationPrescribedEvent(request.PatientId, request.Medications)
            {
                AggregateId = dossierAggregateId,
                Type = nameof(DossierMedicationPrescribedEvent),
                Payload = JsonSerializer.Serialize(request),
                Version = latestVersion,
            };

            var result = await eventStore
                .AddEvent(
                    medicationPrescribedEvent,
                    cancellationToken);

            if (!result) return;

            producer.Produce(
                EventHelper.MapEventToRoutingKey(medicationPrescribedEvent.GetType().Name),
                medicationPrescribedEvent.GetType().Name,
                JsonSerializer.Serialize(medicationPrescribedEvent));
        }
    }
}
