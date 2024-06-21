using PatientManagement.Common.Abstractions;

namespace PatientManagement.Common.Entities
{
    public class StoredEvent 
        : Event
    {
        public StoredEvent(Guid aggregateId, string type, string payload, int version) 
            : base(aggregateId, type, payload, version) 
        { 
        }
    }
}
