using DossierManagement.Features.Dossier.CreateDossier;
using DossierManagement.Infrastructure.Persistence.Contexts;
using DossierManagement.Infrastructure.Persistence.Stores;
using MediatR;
using System.Data;

namespace DossierManagement.Events.PatientRegistered
{
    public sealed class PatientRegisteredEventHandler(
        ISender sender,
        IEventStore eventStore,
        ApplicationDbContext context)
        : INotificationHandler<PatientRegisteredEvent>
    {
        public async Task Handle(
            PatientRegisteredEvent notification,
            CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (await eventStore.PatientExists(notification.Patient.Id, cancellationToken))
                    throw new DuplicateNameException($"Patient #{notification.Patient.Id} already exists");

                var command = new CreateDossierCommand(notification.Patient.Id, notification.Patient);
                var dossier = await sender.Send(command, cancellationToken);

                notification.AggregateId = dossier.Id;

                await eventStore.AddEvent(notification, cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }
    }
}
