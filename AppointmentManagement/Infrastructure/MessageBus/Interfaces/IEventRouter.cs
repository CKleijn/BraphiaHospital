using RabbitMQ.Client.Events;

namespace AppointmentManagement.Infrastructure.MessageBus.Interfaces
{
    public interface IEventRouter
    {
        Task RouteEvents(BasicDeliverEventArgs eventArgs);
    }
}
