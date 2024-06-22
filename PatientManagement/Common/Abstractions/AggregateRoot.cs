using PatientManagement.Common.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Common.Aggregates
{
    public abstract class AggregateRoot
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public int Version { get; set; } = 0;

        private void ApplyChange(Event @event)
        {
            dynamic currentAggregateRoot = this;
            currentAggregateRoot.Apply(@event);
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
