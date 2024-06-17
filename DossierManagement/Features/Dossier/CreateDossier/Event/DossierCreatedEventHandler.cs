using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier.Event
{
    public sealed class DossierCreatedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierCreatedEvent>
    {
        public async Task Handle(
            DossierCreatedEvent notification, 
            CancellationToken cancellationToken)
        {
            context.Set<Dossier>().Add(notification.Dossier);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
