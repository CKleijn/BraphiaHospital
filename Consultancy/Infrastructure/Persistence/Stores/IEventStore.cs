using Consultancy.Common.Abstractions;

namespace Consultancy.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetAllEventsByAggregateId(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> EventByAggregateIdExists(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> AppointmentAlreadyHasConsult(Guid appointmentId, CancellationToken cancellationToken);
        Task<bool> AddEvent(Event @event, CancellationToken cancellationToken);
    }
}
