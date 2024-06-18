using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Events.ResultAppended
{
    public sealed class DossierResultAppendedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierResultAppendedEvent>
    {
        public async Task Handle(
            DossierResultAppendedEvent notification,
            CancellationToken cancellationToken)
        {
            context
                .Set<Dossier>()
                .Update(notification.Dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
