using PatientManagement.Common.Abstractions;

namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetAllEventsByAggregateId(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> PropertyExists(string propertyName, string propertyValue, CancellationToken cancellationToken);
        Task<bool> AddEvent(Event @event, CancellationToken cancellationToken);
    }
}
