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
            var dossier = new Dossier
            {
                Id = notification.Id,
                PatientId = notification.PatientId
            };

            context
                .Set<Dossier>()
                .Add(dossier);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
