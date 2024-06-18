using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Event
{
    public sealed class ConsultCreatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<ConsultCreatedEvent>
    {
        public async Task Handle(
            ConsultCreatedEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Consult>().Add(notification.Consult);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
