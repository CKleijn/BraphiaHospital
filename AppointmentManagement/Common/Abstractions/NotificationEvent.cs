namespace AppointmentManagement.Common.Abstractions
{
    public abstract class NotificationEvent
    {
        public Guid AggregateId { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int Version { get; set; } = 0;
    }
}