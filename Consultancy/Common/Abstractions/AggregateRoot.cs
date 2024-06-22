using Consultancy.Common.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Common.Aggregates
{
    public abstract class AggregateRoot
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public int Version { get; set; } = 0;

        public void ApplyChange<TEvent>(TEvent @event) where TEvent : Event
        {
            Apply(@event);
        }

        private void Apply<TEvent>(TEvent @event)
        {
            dynamic currentAggregateRoot = this;
            currentAggregateRoot.Apply((dynamic) @event!);
        }

        public void ReplayHistory(IEnumerable<Event> history)
        {
            foreach (var @event in history)
            {
                ApplyChange(@event);
                Version = @event.Version;
            }
        }
    }
}
