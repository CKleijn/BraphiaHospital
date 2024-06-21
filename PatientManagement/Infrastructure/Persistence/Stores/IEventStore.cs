using PatientManagement.Common.Abstractions;
using PatientManagement.Common.Entities;

namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<StoredEvent>> GetAllEventsByAggregateId(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> EventByAggregateIdExists(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> BSNExists(string BSN, CancellationToken cancellationToken);
        Task<bool> AddEvent(Event @event, CancellationToken cancellationToken);
    }
}
