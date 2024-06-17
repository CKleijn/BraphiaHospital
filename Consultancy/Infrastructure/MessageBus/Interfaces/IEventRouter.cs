using RabbitMQ.Client.Events;

namespace Consultancy.Infrastructure.MessageBus.Interfaces
{
    public interface IEventRouter
    {
        Task RouteEvents(BasicDeliverEventArgs eventArgs);
    }
}
