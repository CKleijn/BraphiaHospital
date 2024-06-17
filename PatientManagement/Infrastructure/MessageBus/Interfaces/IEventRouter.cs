using RabbitMQ.Client.Events;

namespace PatientManagement.Infrastructure.MessageBus.Interfaces
{
    public interface IEventRouter
    {
        Task RouteEvents(BasicDeliverEventArgs eventArgs);
    }
}
