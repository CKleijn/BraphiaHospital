using DossierManagement.Common.Helpers;
using DossierManagement.Features.Dossier._Interfaces;
using DossierManagement.Features.Dossier.CreateDossier.Event;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.CreateDossier.Command
{
    public sealed class CreateDossierCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        IDossierMapper mapper,
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

            // Check based on eventStore instead of read db?
            if (await context.Set<Patient>()
                .AnyAsync(p => p.Id != request.PatientId, cancellationToken))
                throw new DuplicateNameException($"Patient #{request.PatientId} doesn't exist");

            // Check based on eventStore instead of read db?
            if (await context.Set<Dossier>()
                .Include(d => d.Patient)
                .AnyAsync(d => d.Patient.Id == request.PatientId, cancellationToken))
                throw new DuplicateNameException($"Dossier with patient #{request.PatientId} already exist");

            var dossier = mapper.CreateDossierCommandToDossier(request);

            var result = await eventStore
                .AddEvent(
                    typeof(DossierCreatedEvent).Name,
                    JsonSerializer.Serialize(dossier),
                    cancellationToken);

            if (result)
            {
                var dossierCreatedEvent = mapper.DossierToDossierCreatedEvent(dossier);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(typeof(DossierCreatedEvent).Name),
                    dossierCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(dossierCreatedEvent));
            }
        }
    }
}
