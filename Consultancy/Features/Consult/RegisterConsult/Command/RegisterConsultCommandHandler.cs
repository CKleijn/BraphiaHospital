using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Consultancy.Common.Helpers;
using Consultancy.Features.Consult._Interfaces;
using Consultancy.Features.Consult.RegisterConsult.Event;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Infrastructure.Persistence.Stores;
using System.Data;
using System.Text.Json;

namespace Consultancy.Features.Consult.RegisterConsult.Command
{
    public sealed class RegisterConsultCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<RegisterConsultCommand> validator,
        IConsultMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<RegisterConsultCommand>
    {
        public async Task Handle(
            RegisterConsultCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (await context.Set<Consult>().AnyAsync(c => c.AppointmentId == request.AppointmentId, cancellationToken))
                throw new DuplicateNameException($"{request.AppointmentId} already has a consult");

            var consult = mapper.RegisterConsultCommandToConsult(request);

            var result = await eventStore
                .AddEvent(
                    typeof(ConsultRegisteredEvent).Name,
                    JsonSerializer.Serialize(consult),
                    cancellationToken);

            if (result)
            {
                var consultRegisteredEvent = mapper.ConsultToConsultRegisteredEvent(consult);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(consultRegisteredEvent.GetType().Name),
                    consultRegisteredEvent.GetType().Name,
                    JsonSerializer.Serialize(consultRegisteredEvent));
            }
        }
    }
}
