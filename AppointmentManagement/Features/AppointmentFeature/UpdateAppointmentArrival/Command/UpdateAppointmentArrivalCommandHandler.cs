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
    public sealed class UpdateAppointmentArrivalCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateAppointmentArrivalCommand> validator,
        IAppointmentMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<UpdateAppointmentArrivalCommand>
    {
        public async Task Handle(
            UpdateAppointmentArrivalCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            bool eventExists = await eventStore.EventByAggregateIdExists(request.Id, cancellationToken);

            if (!eventExists)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");

            AppointmentArrivalUpdatedEvent patientArrivalUpdatedEvent = new(request.Id, request.Status)
            {
                AggregateId = request.Id,
                Type = nameof(AppointmentArrivalUpdatedEvent),
                Payload = JsonSerializer.Serialize(request),
                Version = (await eventStore.GetAllEventsByAggregateId(request.Id, null, cancellationToken)).ToList().Last().Version + 1
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
