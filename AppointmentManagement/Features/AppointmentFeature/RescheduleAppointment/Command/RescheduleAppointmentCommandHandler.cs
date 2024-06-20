using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using System.Text.Json;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

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
            //TODO: if partial payload is allowed there will be no need for the context call
            //Move id validation to validator when partial payload will be implemented

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            Appointment? appointment = await context.Set<Appointment>()
                .FindAsync(request.Id, cancellationToken);

            if (appointment == null)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");


            appointment.ScheduledDateTime = request.ScheduledDateTime;


            var result = await eventStore
                .AddEvent(
                    typeof(AppointmentScheduledEvent).Name,
                    JsonSerializer.Serialize(appointment),
                    cancellationToken);

            if (!result)
                return;

            var appointmentRescheduledEvent = mapper.AppointmentToAppointmentRescheduledEvent(appointment);

            producer.Produce(
                EventMapper.MapEventToRoutingKey(appointmentRescheduledEvent.GetType().Name),
                appointmentRescheduledEvent.GetType().Name,
                JsonSerializer.Serialize(appointmentRescheduledEvent));
        }
    }
}