using DossierManagement.Common.Helpers;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.PrescribeMedication
{
    public sealed class PrescribeMedicationCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        ApplicationDbContext context,
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

            if (await context
                .Set<Dossier>()
                .AnyAsync(d => d.Patient.Id != request.PatientId, cancellationToken))
                throw new ArgumentNullException($"Dossier doesn't exist for patient #{request.PatientId}");

            var medicationPrescribedEvent = new DossierMedicationPrescribedEvent
            (
                PatientId: request.PatientId,
                Medications: request.Medications
            );

            var result = await eventStore
                .AddEvent(
                    medicationPrescribedEvent.GetType().Name,
                    JsonSerializer.Serialize(medicationPrescribedEvent),
                    cancellationToken);

            if (result)
            {
                producer.Produce(
                    EventMapper.MapEventToRoutingKey(medicationPrescribedEvent.GetType().Name),
                    medicationPrescribedEvent.GetType().Name,
                    JsonSerializer.Serialize(medicationPrescribedEvent));
            }
        }
    }
}
