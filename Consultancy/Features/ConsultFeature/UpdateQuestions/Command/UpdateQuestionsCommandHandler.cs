using FluentValidation;
using MediatR;
using Consultancy.Common.Helpers;

using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Consultancy.Features.ConsultFeature.UpdateQuestions.Event;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Command
{
    public sealed class UpdateQuestionsCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateQuestionsCommmand> validator,
        IConsultMapper mapper,
        ApplicationDbContext context)
        : IRequestHandler<UpdateQuestionsCommmand>
    {
        public async Task Handle(
            UpdateQuestionsCommmand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            Consult consult = await context.Set<Consult>()
                .Include(c => c.Survey)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ?? throw new KeyNotFoundException($"No consult present with id #{request.Id}");

            if (consult.Survey == null)
                throw new KeyNotFoundException($"No survey present within consult with id #{request.Id}");

            if (!consult.Notes.IsNullOrEmpty())
                throw new InvalidOperationException($"Consult with id #{request.Id} has already finished and therefore cannot be edited");

            await context.Entry(consult.Survey)
                .Collection(s => s.Questions)
                .LoadAsync(cancellationToken);

            foreach (Question questionRequest in request.Questions)
            {
                Question questionContext = consult.Survey.Questions.FirstOrDefault(c => c.Id == questionRequest.Id) 
                    ?? throw new KeyNotFoundException($"No question present with given id #{questionRequest.Id}");

                questionContext.AnswerValue = questionRequest.AnswerValue;
            }

            if (consult.Survey.Questions.Any(q => q.AnswerValue.IsNullOrEmpty()))
                throw new InvalidOperationException("Not all questions in the survey are answered");

            var result = await eventStore
                .AddEvent(
                    typeof(ConsultSurveyFilledInEvent).Name,
                    JsonSerializer.Serialize(consult),
                    cancellationToken);

            if (result)
            {
                var surveyFilledInEvent = mapper.ConsultToConsultSurveyFilledInEvent(consult);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(surveyFilledInEvent.GetType().Name),
                    surveyFilledInEvent.GetType().Name,
                    JsonSerializer.Serialize(surveyFilledInEvent));
            }
        }
    }
}
