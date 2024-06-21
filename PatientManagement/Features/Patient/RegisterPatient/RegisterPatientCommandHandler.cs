using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Common.Helpers;
using PatientManagement.Events;
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

            if (await context.Set<Patient>().AnyAsync(p => p.BSN == request.BSN, cancellationToken))
                throw new DuplicateNameException($"{request.BSN} already exists");

            var patientRegisteredEvent = new PatientRegisteredEvent
            (
                Patient: new Patient
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    BSN = request.BSN,
                    Address = request.Address
                }
            );

            var result = await eventStore
                .AddEvent(
                    patientRegisteredEvent.GetType().Name,
                    JsonSerializer.Serialize(patientRegisteredEvent),
                    cancellationToken);

            if (result)
            {
                producer.Produce(
                    EventMapper.MapEventToRoutingKey(patientRegisteredEvent.GetType().Name),
                    patientRegisteredEvent.GetType().Name,
                    JsonSerializer.Serialize(patientRegisteredEvent));
            }
        }
    }
}
