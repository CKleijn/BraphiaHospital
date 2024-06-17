using PatientManagement.Infrastructure.MessageBus.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace PatientManagement.Infrastructure.MessageBus.Implementations
{
    public class Producer
        : IProducer
    {
        public void Produce(string routingKey, string eventName, string eventMessage)
        {
            var factory = new ConnectionFactory { HostName = Keys.HOST_NAME };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Keys.EVENTS_EXCHANGE, ExchangeType.Topic, true, false, null);

            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
            {
                { "EventName", eventName }
            };
            properties.Persistent = true;

            channel.BasicPublish(Keys.EVENTS_EXCHANGE, routingKey, properties, Encoding.UTF8.GetBytes(eventMessage));

            Console.WriteLine($"Sent: {eventMessage}");
        }
    }
}
