using RabbitMQ.Client.Events;

namespace DossierManagement.Infrastructure.MessageBus.Interfaces
{
    public interface IEventRouter
    {
        Task RouteEvents(BasicDeliverEventArgs eventArgs);
    }
}
