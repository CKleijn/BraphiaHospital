using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;

namespace Consultancy.Features.Consult.RegisterConsult.Event
{
    public sealed class ConsultRegisteredEventHandler(ApplicationDbContext context)
        : INotificationHandler<ConsultRegisteredEvent>
    {
        public async Task Handle(
            ConsultRegisteredEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Consult>().Add(notification.Consult);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
