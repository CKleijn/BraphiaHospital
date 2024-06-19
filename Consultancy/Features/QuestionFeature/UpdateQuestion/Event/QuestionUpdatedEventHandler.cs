using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Azure.Core;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Event
{
    public sealed class QuestionUpdatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<QuestionUpdatedEvent>
    {
        public async Task Handle(
            QuestionUpdatedEvent notification, 
            CancellationToken cancellationToken)
        {
            Question? question = await context.Set<Question>()
                .FindAsync(notification.Question.Id, cancellationToken);

            if (question == null)
                throw new ArgumentNullException($"Question #{notification.Question.Id} doesn't exist");

            context.Set<Question>().Update(notification.Question);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
