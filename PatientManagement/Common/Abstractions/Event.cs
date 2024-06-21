namespace PatientManagement.Common.Abstractions
{
    public abstract class Event
    {
        public Guid AggregateId { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int Version { get; set; } = 0;

        //public Event(Guid aggregateId, string type, string payload, int version)
        //{
        //    AggregateId = aggregateId;
        //    Type = type;
        //    Payload = payload;
        //    Version = version;
        //}

        //public Event(string type, string payload, int version)
        //{
        //    Type = type;
        //    Payload = payload;
        //    Version = version;
        //}
    }
}
