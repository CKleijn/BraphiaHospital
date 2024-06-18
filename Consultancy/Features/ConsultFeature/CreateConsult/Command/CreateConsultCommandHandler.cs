using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Consultancy.Common.Helpers;
using Consultancy.Features.ConsultFeature._Interfaces;

using System.Data;
using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.CreateConsult.Event;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Command
{
    public sealed class CreateConsultCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<CreateConsultCommand> validator,
        IConsultMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<CreateConsultCommand>
    {
        public async Task Handle(
            CreateConsultCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (await context.Set<Consult>().AnyAsync(c => c.AppointmentId == request.AppointmentId, cancellationToken))
                throw new DuplicateNameException($"{request.AppointmentId} already has a consult");

            if (await context.Set<Consult>().AnyAsync(c => c.Survey.Id == request.Survey.Id, cancellationToken))
                throw new DuplicateNameException($"Unable to create new survey with already existing survey id");

            var existingQuestionIds = await context.Set<Consult>()
                .SelectMany(c => c.Survey.Questions.Select(q => q.Id))
                .ToListAsync();

            if (request.Survey.Questions.Any(rq => existingQuestionIds.Contains(rq.Id)))
            {
                throw new DuplicateNameException($"Unable to create new question with already existing question id");
            }

            var consult = mapper.CreateConsultCommandToConsult(request);

            var result = await eventStore
                .AddEvent(
                    typeof(ConsultCreatedEvent).Name,
                    JsonSerializer.Serialize(consult),
                    cancellationToken);

            if (result)
            {
                var consultCreatedEvent = mapper.ConsultToConsultCreatedEvent(consult);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(consultCreatedEvent.GetType().Name),
                    consultCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(consultCreatedEvent));
            }
        }
    }
}
