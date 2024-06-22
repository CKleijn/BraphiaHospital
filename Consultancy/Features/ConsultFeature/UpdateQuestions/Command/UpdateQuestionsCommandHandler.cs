using FluentValidation;
using MediatR;

using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Entities;
using Microsoft.IdentityModel.Tokens;
using Consultancy.Common.Abstractions;
using Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent;
using Consultancy.Common.Helpers;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Command
{
    public sealed class UpdateQuestionsCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateQuestionsCommmand> validator)
        : IRequestHandler<UpdateQuestionsCommmand>
    {
        public async Task Handle(
            UpdateQuestionsCommmand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            IEnumerable<Event> aggregateEvents = await eventStore.GetAllEventsByAggregateId(request.Id, cancellationToken);
            
            if (aggregateEvents.IsNullOrEmpty())
                throw new KeyNotFoundException($"No consult present with id #{request.Id}");

            Consult consult = new() { Id = request.Id };
            consult.ReplayHistory(aggregateEvents);

            if (!consult.Notes.IsNullOrEmpty())
                throw new InvalidOperationException($"Consult with id #{consult.Id} has already finished and therefore cannot be edited");

            if (consult.Survey == null)
                throw new KeyNotFoundException($"No survey present within consult with id #{request.Id}");

            foreach (Question questionRequest in request.Questions)
            {
                Question questions = consult.Survey.Questions.FirstOrDefault(c => c.Id == questionRequest.Id) 
                    ?? throw new KeyNotFoundException($"No question present with given id #{questionRequest.Id}");

                questions.AnswerValue = questionRequest.AnswerValue;
            }

            if (consult.Survey.Questions.Any(q => q.AnswerValue.IsNullOrEmpty()))
                throw new InvalidOperationException("Not all questions in the survey are answered");

            consult.Version++;
            ConsultSurveyFilledInEvent consultSurveyFilledInEvent = new (consult.Survey.Questions)
            {
                AggregateId = consult.Id,
                Type = nameof(ConsultSurveyFilledInEvent),
                Payload = JsonSerializer.Serialize(consult.Survey.Questions),
                Version = consult.Version
            };

            var result = await eventStore
                .AddEvent(
                    consultSurveyFilledInEvent,
                    cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventHelper.MapEventToRoutingKey(consultSurveyFilledInEvent.GetType().Name),
                consultSurveyFilledInEvent.GetType().Name,
                JsonSerializer.Serialize(consultSurveyFilledInEvent));
        }
    }
}
