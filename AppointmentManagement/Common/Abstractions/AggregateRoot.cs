using AppointmentManagement.Common.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Aggregates
{
    public abstract class AggregateRoot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Version { get; set; } = 0;

        public void ApplyChange<TEvent>(TEvent @event) where TEvent : NotificationEvent
        {
            Apply(@event);
        }

        private void Apply<TEvent>(TEvent @event)
        {
            dynamic currentAggregateRoot = this;
            currentAggregateRoot.Apply((dynamic)@event!);
        }

        public void ReplayHistory(IEnumerable<NotificationEvent> history)
        {
            foreach (var @event in history)
            {
                ApplyChange(@event);
                Version = @event.Version;
            }
        }
    }
}
