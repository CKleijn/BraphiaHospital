using DossierManagement.Common.Helpers;
using DossierManagement.Events.ResultAppended;
using DossierManagement.Features.Dossier._Interfaces;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DossierManagement.Features.Dossier.AppendResult
{
    public sealed class AppendResultCommandHandler(
        IEventStore eventStore,
        IProducer producer,
        IDossierMapper mapper,
        ApplicationDbContext context,
        IValidator<AppendResultCommand> validator)
        : IRequestHandler<AppendResultCommand>
    {
        public async Task Handle(
            AppendResultCommand request,
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
                .Set<Result>()
                .AnyAsync(c => c.Id != request.Result.Id, cancellationToken))
                throw new ArgumentNullException($"Result #{request.Result.Id} doesn't exist");

            if (await context
                .Set<Dossier>()
                .Include(d => d.Results)
                .AnyAsync(d => d.Results != null && d.Results.Any(c => c.Id == request.Result.Id), cancellationToken))
                throw new DuplicateNameException($"Result #{request.Result.Id} is already appended");

            dossier.Results!.Add(request.Result);

            var result = await eventStore
                .AddEvent(
                    typeof(DossierResultAppendedEvent).Name,
                    JsonSerializer.Serialize(dossier),
                    cancellationToken);

            if (result)
            {
                var resultAppendedEvent = mapper.DossierToResultAppendedEvent(dossier);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(typeof(DossierResultAppendedEvent).Name),
                    resultAppendedEvent.GetType().Name,
                    JsonSerializer.Serialize(resultAppendedEvent));
            }
        }
    }
}
