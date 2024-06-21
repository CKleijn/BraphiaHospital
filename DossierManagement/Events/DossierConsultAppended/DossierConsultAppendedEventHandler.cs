using DossierManagement.Features.Dossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Events.ConsultAppended
{
    public sealed class DossierConsultAppendedEventHandler(ApplicationDbContext context)
        : INotificationHandler<DossierConsultAppendedEvent>
    {
        public async Task Handle(
            DossierConsultAppendedEvent notification,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .FirstOrDefaultAsync(d => d.Patient.Id == notification.PatientId, cancellationToken)
                ?? throw new ArgumentNullException($"Dossier doesn't exist for patient #{notification.PatientId}");

            if (await context
                .Set<Dossier>()
                .AnyAsync(d => d.Consults.Any(c => c.Id == notification.Consult.Id), cancellationToken) 
                && await context
                .Set<Consult>()
                .AnyAsync(c => c.Id == notification.Consult.Id, cancellationToken))
                throw new ArgumentNullException($"Consult #{notification.Consult.Id} already exist");

            dossier.Consults ??= new List<Consult>();
            dossier.Consults.Add(notification.Consult);

            context.Entry(dossier).State = EntityState.Modified;
            context.Entry(notification.Consult).State = EntityState.Added;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
