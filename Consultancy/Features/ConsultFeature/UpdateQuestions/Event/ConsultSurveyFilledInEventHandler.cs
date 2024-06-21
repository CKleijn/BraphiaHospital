using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Event
{
    public sealed class ConsultSurveyFilledInEventHandler(ApplicationDbContext context)
        : INotificationHandler<ConsultSurveyFilledInEvent>
    {
        public async Task Handle(
            ConsultSurveyFilledInEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult? consultToBeUpdated = await context.Set<Consult>()
                .Include(c => c.Survey)
                .Include(c => c.Survey!.Questions)
                .FirstOrDefaultAsync(c => c.Id == notification.Consult.Id, cancellationToken);

            foreach (Question incomingQuestion in notification.Consult.Survey!.Questions)
            {
                Question existingQuestion = consultToBeUpdated!.Survey!.Questions.FirstOrDefault(c => c.Id == incomingQuestion.Id)!;

                existingQuestion.AnswerValue = incomingQuestion.AnswerValue;
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
