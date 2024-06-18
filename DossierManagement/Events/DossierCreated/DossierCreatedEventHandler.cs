using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Events.DossierCreated
{
    public sealed class DossierCreatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierCreatedEvent>
    {
        public async Task Handle(
            DossierCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            context
                .Set<Dossier>()
                .Add(notification.Dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
