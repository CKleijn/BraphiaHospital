using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Common.Helpers;
using PatientManagement.Events;
using PatientManagement.Features.Patient._Interfaces;
using PatientManagement.Infrastructure.MessageBus.Interfaces;
using PatientManagement.Infrastructure.Persistence.Contexts;
using PatientManagement.Infrastructure.Persistence.Stores;
using System.Data;
using System.Text.Json;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed class RegisterPatientCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<RegisterPatientCommand> validator,
        IPatientMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<RegisterPatientCommand>
    {
        public async Task Handle(
            RegisterPatientCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Check based on eventStore instead of read db?
            if (await context.Set<Patient>().AnyAsync(p => p.BSN == request.BSN, cancellationToken))
                throw new DuplicateNameException($"{request.BSN} already exists");

            var patient = mapper.RegisterPatientCommandToPatient(request);

            var result = await eventStore
                .AddEvent(
                    typeof(PatientRegisteredEvent).Name,
                    JsonSerializer.Serialize(patient),
                    cancellationToken);

            if (result)
            {
                var patientRegisteredEvent = mapper.PatientToPatientRegisteredEvent(patient);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(patientRegisteredEvent.GetType().Name),
                    patientRegisteredEvent.GetType().Name,
                    JsonSerializer.Serialize(patientRegisteredEvent));
            }
        }
    }
}
