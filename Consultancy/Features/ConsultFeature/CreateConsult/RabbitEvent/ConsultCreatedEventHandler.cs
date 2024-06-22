using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Common.Helpers;

namespace Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent
{
    public sealed class ConsultCreatedEventHandler(
        ApplicationDbContext context)
        : INotificationHandler<ConsultCreatedEvent>
    {
        public async Task Handle(
            ConsultCreatedEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Consult>().Add(notification.Consult);
            await context.SaveChangesAsync(cancellationToken);

            ContextDetacher.DetachAllEntitiesFromContext(context);
        }
    }
}
