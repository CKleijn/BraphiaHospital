using PatientManagement.Common.Abstractions;

namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetAllEventsByAggregateId(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> EventByAggregateIdExists(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> BSNExists(string BSN, CancellationToken cancellationToken);
        Task<bool> AddEvent(Event @event, CancellationToken cancellationToken);
    }
}
