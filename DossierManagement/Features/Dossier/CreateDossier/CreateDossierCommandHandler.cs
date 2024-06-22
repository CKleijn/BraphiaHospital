using DossierManagement.Common.Helpers;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.CreateDossier
{
    public sealed class CreateDossierCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        IValidator<CreateDossierCommand> validator)
        : IRequestHandler<CreateDossierCommand, Dossier>
    {
        public async Task<Dossier> Handle(
            CreateDossierCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (await eventStore.DossierWithPatientExists(request.PatientId, cancellationToken))
                throw new DuplicateNameException($"Dossier with patient #{request.PatientId} already exists");

            var dossier = new Dossier
            {
                PatientId = request.PatientId,
                Patient = request.Patient
            };
            
            var dossierCreatedEvent = new DossierCreatedEvent(dossier)
            {
                AggregateId = dossier.Id,
                Type = nameof(DossierCreatedEvent),
                Payload = JsonSerializer.Serialize(dossier),
                Version = 0
            };

            var result = await eventStore
                .AddEvent(
                    dossierCreatedEvent,
                    cancellationToken);

            if (result)
            {
                producer.Produce(
                EventHelper.MapEventToRoutingKey(dossierCreatedEvent.GetType().Name),
                dossierCreatedEvent.GetType().Name,
                JsonSerializer.Serialize(dossierCreatedEvent));
            }

            return dossier;
        }
    }
}
