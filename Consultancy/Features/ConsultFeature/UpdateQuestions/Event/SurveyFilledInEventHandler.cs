using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Event
{
    public sealed class SurveyFilledInEventHandler(ApplicationDbContext context)
        : INotificationHandler<SurveyFilledInEvent>
    {
        public async Task Handle(
            SurveyFilledInEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult existingConsult = await context.Set<Consult>()
                .Include(c => c.Survey)
                .FirstOrDefaultAsync(c => c.Id == notification.Consult.Id, cancellationToken) ?? throw new KeyNotFoundException($"No consult present with id #{notification.Consult.Id}");

            if (existingConsult.Survey == null)
                throw new KeyNotFoundException($"No survey present within consult with id #{notification.Consult.Id}");

            if (existingConsult.Notes.IsNullOrEmpty()!)
                throw new InvalidOperationException($"Consult with id #{notification.Consult.Id} has already finished and therefore cannot be edited");

            await context.Entry(existingConsult.Survey)
                .Collection(s => s.Questions)
                .LoadAsync(cancellationToken);

            foreach (Question incomingQuestion in notification.Consult.Survey!.Questions)
            {
                Question existingQuestion = existingConsult.Survey.Questions.FirstOrDefault(c => c.Id == incomingQuestion.Id)
                    ?? throw new KeyNotFoundException($"No question present with given id #{incomingQuestion.Id}");

                existingQuestion.AnswerValue = incomingQuestion.AnswerValue;
            }

            if (existingConsult.Survey.Questions.Any(q => q.AnswerValue == null))
                throw new InvalidOperationException("Not all questions in the survey are answered");

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
