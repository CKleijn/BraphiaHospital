using DossierManagement.Common.Helpers;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Features.Dossier._Interfaces;
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

            var patient = await context
                .Set<Patient>()
                .FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

            if (patient == null)
                throw new ArgumentNullException($"Patient #{request.PatientId} doesn't exist");

            if (await context
                .Set<Dossier>()
                .Include(d => d.Patient)
                .AnyAsync(d => d.Patient.Id == request.PatientId, cancellationToken))
                throw new DuplicateNameException($"Dossier with patient #{request.PatientId} already exist");

            var dossier = mapper.PatientToDossier(patient);

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
