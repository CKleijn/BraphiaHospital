using DossierManagement.Common.Abstractions;

namespace DossierManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetAllEventsByAggregateId(Guid aggregateId, CancellationToken cancellationToken);
        Task<Guid> GetDossierAggregateIdByPatientId(Guid patientId, CancellationToken cancellationToken);
        Task<int> GetLatestVersionOfEventByAggregateId(Guid aggregateId, string @event, CancellationToken cancellationToken);
        Task<bool> EventByAggregateIdExists(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> DossierWithPatientExists(Guid patientId, CancellationToken cancellationToken);
        Task<bool> PatientExists(Guid patientId, CancellationToken cancellationToken);
        Task<bool> AddEvent(Event @event, CancellationToken cancellationToken);
    }
}
