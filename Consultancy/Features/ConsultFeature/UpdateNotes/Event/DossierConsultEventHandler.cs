using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Event
{
    public sealed class DossierConsultAppendedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification, 
            CancellationToken cancellationToken)
        {
            Consult? consultToUpdate = await context.Set<Consult>()
                .FindAsync(notification.Consult.Id, cancellationToken);

            consultToUpdate!.Notes = notification.Consult.Notes;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
