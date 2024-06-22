using AppointmentManagement.Common.Abstractions;

namespace AppointmentManagement.Common.Entities
{
    public class StoredEvent 
        : NotificationEvent
    {
        public StoredEvent(Guid aggregateId, string type, string payload, int version) 
        { 
            AggregateId = aggregateId;
            Type = type;
            Payload = payload;
            Version = version;
        }
    }
}
