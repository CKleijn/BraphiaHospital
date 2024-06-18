using FluentValidation;
using MediatR;
using Consultancy.Common.Helpers;
using Consultancy.Features.ConsultFeature._Interfaces;

using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Event;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Contexts;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Command
{
    public sealed class UpdateQuestionCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateQuestionCommmand> validator,
        IQuestionMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<UpdateQuestionCommmand>
    {
        public async Task Handle(
            UpdateQuestionCommmand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            Question? question = await context.Set<Question>()
                .FindAsync(request.Id, cancellationToken);

            if (question == null)
                throw new ArgumentNullException($"Question #{request.Id} doesn't exist");

            question.AnswerValue = request.AnswerValue;

            var result = await eventStore
                .AddEvent(
                    typeof(QuestionUpdatedEvent).Name,
                    JsonSerializer.Serialize(question),
                    cancellationToken);

            if (result)
            {
                var questionUpdatedEvent = mapper.QuestionToQuestionUpdatedEvent(question);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(questionUpdatedEvent.GetType().Name),
                    questionUpdatedEvent.GetType().Name,
                    JsonSerializer.Serialize(questionUpdatedEvent));
            }
        }
    }
}
