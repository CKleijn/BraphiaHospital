using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Stores;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent
{
    public sealed class ConsultSurveyFilledInEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<ConsultSurveyFilledInEvent>
    {
        public async Task Handle(
            ConsultSurveyFilledInEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult? consult = await context.Set<Consult>().FindAsync(notification.AggregateId, cancellationToken);
            consult?.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken));

            foreach (Question incomingQuestion in notification.Consult.Survey!.Questions)
            {
                Question existingQuestion = consult?.Survey!.Questions.FirstOrDefault(c => c.Id == incomingQuestion.Id)!;

                existingQuestion.AnswerValue = incomingQuestion.AnswerValue;
            }

            consult!.Version = notification.Consult.Version++;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
