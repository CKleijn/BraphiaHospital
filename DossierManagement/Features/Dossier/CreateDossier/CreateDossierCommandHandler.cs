using DossierManagement.Common.Helpers;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.CreateDossier
{
    public sealed class CreateDossierCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        ApplicationDbContext context,
        IValidator<CreateDossierCommand> validator)
        : IRequestHandler<CreateDossierCommand>
    {
        public async Task Handle(
            CreateDossierCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

           if (await context
                .Set<Patient>()
                .AnyAsync(p => p.Id != request.PatientId, cancellationToken))
                throw new ArgumentNullException($"Patient #{request.PatientId} doesn't exist");

            if (await context
                .Set<Dossier>()
                .Include(d => d.Patient)
                .AnyAsync(d => d.Patient.Id == request.PatientId, cancellationToken))
                throw new DuplicateNameException($"Dossier with patient #{request.PatientId} already exist");

            var dossierCreatedEvent = new DossierCreatedEvent
            (
                Id: Guid.NewGuid(),
                PatientId: request.PatientId
            );

            var result = await eventStore
                .AddEvent(
                    dossierCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(dossierCreatedEvent),
                    cancellationToken);

            if (result)
            {
                producer.Produce(
                    EventMapper.MapEventToRoutingKey(dossierCreatedEvent.GetType().Name),
                    dossierCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(dossierCreatedEvent));
            }
        }
    }
}
