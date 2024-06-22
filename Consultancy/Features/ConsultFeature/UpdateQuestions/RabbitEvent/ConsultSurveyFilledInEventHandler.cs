using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Helpers;

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

            consult!.Version = notification.Version++;

            await context.SaveChangesAsync(cancellationToken);

            ContextDetacher.DetachAllEntitiesFromContext(context);
        }
    }
}
