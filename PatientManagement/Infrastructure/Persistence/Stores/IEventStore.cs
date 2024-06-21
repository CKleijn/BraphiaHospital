namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<TEntity>> GetAllEventsByEvent<TEvent, TEntity>(CancellationToken cancellationToken);
        Task<bool> AddEvent(string eventKey, string eventValue, CancellationToken cancellationToken);
    }
}
