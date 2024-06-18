using DossierManagement.Common.Helpers;
using DossierManagement.Events.ConsultAppended;
using DossierManagement.Features.Dossier._Interfaces;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.AppendConsult
{
    public sealed class AppendConsultCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        IDossierMapper mapper,
        ApplicationDbContext context,
        IValidator<AppendConsultCommand> validator)
        : IRequestHandler<AppendConsultCommand>
    {
        public async Task Handle(
            AppendConsultCommand request,
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
                .Set<Consult>()
                .AnyAsync(c => c.Id != request.Consult.Id, cancellationToken))
                throw new ArgumentNullException($"Consult #{request.Consult.Id} doesn't exist");

            if (await context
                .Set<Dossier>()
                .Include(d => d.Consults)
                .AnyAsync(d => d.Consults != null && d.Consults.Any(c => c.Id == request.Consult.Id), cancellationToken))
                throw new DuplicateNameException($"Consult #{request.Consult.Id} is already appended");

            dossier.Consults!.Add(request.Consult);

            var result = await eventStore
                .AddEvent(
                    typeof(DossierConsultAppendedEvent).Name,
                    JsonSerializer.Serialize(dossier),
                    cancellationToken);

            if (result)
            {
                var consultAppendedEvent = mapper.DossierToConsultAppendedEvent(dossier);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(typeof(DossierConsultAppendedEvent).Name),
                    consultAppendedEvent.GetType().Name,
                    JsonSerializer.Serialize(consultAppendedEvent));
            }
        }
    }
}
