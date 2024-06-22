namespace Consultancy.Common.Abstractions
{
    public abstract class Event
    {
        public Guid AggregateId { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int Version { get; set; } = 0;
    }
}
