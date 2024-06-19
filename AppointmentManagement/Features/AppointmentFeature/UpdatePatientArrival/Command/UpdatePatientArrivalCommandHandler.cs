using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
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
            //TODO: if partial payload is allowed there will be no need for the context call
            //Move id validation to validator when partial payload will be implemented

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            Appointment? appointment = await context.Set<Appointment>()
                .FindAsync(request.Id, cancellationToken);

            if (appointment == null)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");

            appointment.Status = request.Status;

            var result = await eventStore
              .AddEvent(
                  typeof(PatientArrivalUpdatedEvent).Name,
                  JsonSerializer.Serialize(appointment),
                  cancellationToken);

            if (!result)
                return;

            var updatePatientArrivalEvent = mapper.AppointmentToPatientArrivalUpdatedEvent(appointment);

            producer.Produce(
                EventMapper.MapEventToRoutingKey(updatePatientArrivalEvent.GetType().Name),
                updatePatientArrivalEvent.GetType().Name,
                JsonSerializer.Serialize(updatePatientArrivalEvent));

        }
    }
}
