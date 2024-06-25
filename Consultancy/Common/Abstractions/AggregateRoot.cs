using Consultancy.Common.Abstractions;

namespace Consultancy.Common.Aggregates
{
    public abstract class AggregateRoot
    {
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
