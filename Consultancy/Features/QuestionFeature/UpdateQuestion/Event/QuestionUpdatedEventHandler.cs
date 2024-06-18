using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Event
{
    public sealed class QuestionUpdatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<QuestionUpdatedEvent>
    {
        public async Task Handle(
            QuestionUpdatedEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Question>().Update(notification.Question);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
