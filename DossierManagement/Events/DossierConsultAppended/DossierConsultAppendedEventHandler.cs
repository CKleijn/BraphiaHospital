using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed class DossierConsultAppendedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification,
            CancellationToken cancellationToken)
        {
            context
                .Set<Dossier>()
                .Update(notification.Dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
