using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppointmentManagement.Infrastructure.MessageBus.Implementations
{
    public sealed class Consumer(IEventRouter eventRouter)
        : IConsumer
    {
        public void Consume()
        {
            var factory = new ConnectionFactory { HostName = Keys.HOST_NAME, Port = Keys.PORT_NAME };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Keys.EVENTS_EXCHANGE, ExchangeType.Topic, true, false, null);

            channel.QueueDeclare(Keys.APPOINTMENT_QUEUE, true, false, false, null);

            channel.QueueBind(Keys.APPOINTMENT_QUEUE, Keys.EVENTS_EXCHANGE, Keys.APPOINTMENT_ROUTING_KEY);

            channel.BasicQos(0, 1, false);

            Console.WriteLine("Waiting for events...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, eventArgs) =>
            {
                await eventRouter.RouteEvents(eventArgs);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(Keys.APPOINTMENT_QUEUE, false, consumer);

            Console.ReadLine();
        }
    }
}
