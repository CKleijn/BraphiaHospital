namespace Consultancy.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<bool> AddEvent(string eventKey, string eventValue, CancellationToken cancellationToken);
    }
}
