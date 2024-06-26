using FluentValidation;
using MediatR;
using Consultancy.Common.Helpers;

using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Entities;
using Microsoft.IdentityModel.Tokens;
using Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent;
using Consultancy.Common.Abstractions;
using Consultancy.Common.Entities.DTO;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Command
{
    public sealed class UpdateNotesCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateNotesCommand> validator,
        IApiClient apiClient)
        : IRequestHandler<UpdateNotesCommand>
    {
        public async Task Handle(
            UpdateNotesCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            IEnumerable<Event> aggregateEvents = await eventStore.GetAllEventsByAggregateId(request.Id, cancellationToken);

            if (aggregateEvents.IsNullOrEmpty())
                throw new KeyNotFoundException($"No consult present with id #{request.Id}");

            Consult consult = new() { Id = request.Id };
            consult.ReplayHistory(aggregateEvents);

            if (!consult.Notes.IsNullOrEmpty())
                throw new InvalidOperationException($"Consult with id #{consult.Id} has already finished and therefore cannot be edited");

            _ = await apiClient
               .GetAsync<AppointmentDTO>($"{ConfigurationHelper.GetAppointmentManagementServiceConnectionString()}/appointment/{consult.AppointmentId}", cancellationToken)
               ?? throw new KeyNotFoundException($"Appointment #{consult.AppointmentId} doesn't exist");

            consult.Notes = request.Notes;
            consult.Version++;
            DossierConsultAppendedEvent dossierConsultAppendedEvent = new (consult)
            {
                AggregateId = request.Id,
                Type = nameof(DossierConsultAppendedEvent),
                Payload = JsonSerializer.Serialize(consult),
                Version = consult.Version
            };

            var result = await eventStore
                .AddEvent(
                    dossierConsultAppendedEvent,
                    cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventHelper.MapEventToRoutingKey(dossierConsultAppendedEvent.GetType().Name),
                dossierConsultAppendedEvent.GetType().Name,
                JsonSerializer.Serialize(dossierConsultAppendedEvent));
        }
    }
}
