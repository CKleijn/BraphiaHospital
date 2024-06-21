using FluentValidation;
using MediatR;
using PatientManagement.Common.Helpers;
using PatientManagement.Events;
using PatientManagement.Infrastructure.MessageBus.Interfaces;
using PatientManagement.Infrastructure.Persistence.Stores;
using System.Data;
using System.Text.Json;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed class RegisterPatientCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<RegisterPatientCommand> validator)
        : IRequestHandler<RegisterPatientCommand>
    {
        public async Task Handle(
            RegisterPatientCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!await eventStore.BSNExists(request.BSN, cancellationToken))
                throw new DuplicateNameException($"{request.BSN} already exists");

            var patient = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                BSN = request.BSN,
                Address = request.Address
            };

            var patientRegisteredEvent = new PatientRegisteredEvent(patient)
            {
                AggregateId = patient.Id,
                Type = nameof(PatientRegisteredEvent),
                Payload = JsonSerializer.Serialize(patient)
            };

            var result = await eventStore
                .AddEvent(
                    patientRegisteredEvent,
                    cancellationToken);

            if (!result) return;
                
            producer.Produce(
                EventHelper.MapEventToRoutingKey(patientRegisteredEvent.GetType().Name),
                patientRegisteredEvent.GetType().Name,
                JsonSerializer.Serialize(patientRegisteredEvent));
        }
    }
}
