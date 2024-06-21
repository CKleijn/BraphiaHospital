using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using System.Text.Json;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command
{
    public sealed class UpdatePatientArrivalCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdatePatientArrivalCommand> validator,
        IAppointmentMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<UpdatePatientArrivalCommand>
    {
        public async Task Handle(
            UpdatePatientArrivalCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            bool eventExists = await eventStore.EventByAggregateIdExists(request.Id, cancellationToken);

            if (!eventExists)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");

            PatientArrivalUpdatedEvent patientArrivalUpdatedEvent = new(request.Id, request.Status)
            {
                AggregateId = request.Id,
                Type = nameof(PatientArrivalUpdatedEvent),
                Payload = JsonSerializer.Serialize(request),
                Version = Utils.GetHighestVersionByType<PatientArrivalUpdatedEvent>((await eventStore.GetAllEventsByAggregateId(request.Id, cancellationToken)).ToList()) + 1
            };

            var result = await eventStore
              .AddEvent(
                  patientArrivalUpdatedEvent,
                  cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventMapper.MapEventToRoutingKey(patientArrivalUpdatedEvent.GetType().Name),
                patientArrivalUpdatedEvent.GetType().Name,
                JsonSerializer.Serialize(patientArrivalUpdatedEvent));
        }
    }
}
