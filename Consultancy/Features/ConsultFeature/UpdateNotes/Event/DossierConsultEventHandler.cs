using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Event
{
    public sealed class DossierConsultAppendedEventHandler(ApplicationDbContext context, IApiClient apiClient)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification, 
            CancellationToken cancellationToken)
        {
            if (!await context.Set<Consult>().AnyAsync(c => c.Id == notification.Consult.Id, cancellationToken))
                throw new KeyNotFoundException($"No consult present with id #{notification.Consult.Id}");

            if (await context.Set<Consult>().AnyAsync(c => c.Id == notification.Consult.Id && c.Notes.IsNullOrEmpty()!, cancellationToken))
                throw new InvalidOperationException($"Consult with id #{notification.Consult.Id} has already finished and therefore cannot be edited");

            context.Set<Consult>().Add(notification.Consult);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
