using DossierManagement.Common.Helpers;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Features.Dossier._Interfaces;
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
        IDossierMapper mapper,
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

            var dossier = await context
                .Set<Dossier>()
                .FirstOrDefaultAsync(d => d.Patient.Id == request.PatientId, cancellationToken);

            if (dossier == null)
                throw new ArgumentNullException($"Dossier doesn't exist for patient #{request.PatientId}");

            if (await context
                .Set<Medication>()
                .AnyAsync(c => c.Id != request.Medication.Id, cancellationToken))
                throw new ArgumentNullException($"Medication #{request.Medication.Id} doesn't exist");

            if (await context
                .Set<Dossier>()
                .Include(d => d.Medications)
                .AnyAsync(d => d.Medications != null && d.Medications.Any(c => c.Id == request.Medication.Id), cancellationToken))
                throw new DuplicateNameException($"Medication #{request.Medication.Id} is already appended");

            dossier.Medications!.Add(request.Medication);

            var result = await eventStore
                .AddEvent(
                    typeof(DossierMedicationPrescribedEvent).Name,
                    JsonSerializer.Serialize(dossier),
                    cancellationToken);

            if (result)
            {
                var medicationPrescribedEvent = mapper.DossierToMedicationPrescribedEvent(dossier);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(typeof(DossierMedicationPrescribedEvent).Name),
                    medicationPrescribedEvent.GetType().Name,
                    JsonSerializer.Serialize(medicationPrescribedEvent));
            }
        }
    }
}
