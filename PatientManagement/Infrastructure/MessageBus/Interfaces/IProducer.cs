namespace PatientManagement.Infrastructure.MessageBus.Interfaces
{
    public interface IProducer
    {
        void Produce(string routingKey, string eventName, string eventMessage);
    }
}
