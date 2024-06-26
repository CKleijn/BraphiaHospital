using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Common.Helpers;
using Consultancy.Infrastructure.Persistence.Stores;

namespace Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent
{
    public sealed class ConsultCreatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<ConsultCreatedEvent>
    {
        public async Task Handle(
            ConsultCreatedEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult consult = new();
            consult.ReplayHistory(await eventStore.GetAllEventsByAggregateId(notification.AggregateId, cancellationToken));

            context.Set<Consult>().Add(consult);
            await context.SaveChangesAsync(cancellationToken);

            ContextDetacher.DetachAllEntitiesFromContext(context);
        }
    }
}
