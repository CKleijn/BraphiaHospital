using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using System.Text.Json;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Infrastructure.Persistence.Contexts;

namespace AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command
{
    public sealed class RescheduleAppointmentCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<RescheduleAppointmentCommand> validator,
        IAppointmentMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<RescheduleAppointmentCommand>
    {
        public async Task Handle(
            RescheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            bool eventExists = await eventStore.EventByAggregateIdExists(request.Id, cancellationToken);

            if (!eventExists)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");

            AppointmentRescheduledEvent appointmentRescheduledEvent = new(request.Id, request.ScheduledDateTime)
            {
                AggregateId = request.Id,
                Type = nameof(AppointmentRescheduledEvent),
                Payload = JsonSerializer.Serialize(request),
                Version = Utils.GetHighestVersionByType<AppointmentRescheduledEvent>((await eventStore.GetAllEventsByAggregateId(request.Id, cancellationToken)).ToList()) + 1
            };

            var result = await eventStore
              .AddEvent(
                  appointmentRescheduledEvent,
                  cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventMapper.MapEventToRoutingKey(appointmentRescheduledEvent.GetType().Name),
                appointmentRescheduledEvent.GetType().Name,
                JsonSerializer.Serialize(appointmentRescheduledEvent));
        }
    }
}