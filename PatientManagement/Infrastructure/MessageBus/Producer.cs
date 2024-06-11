using RabbitMQ.Client;
using System.Text;

namespace PatientManagement.Infrastructure.MessageBus
{
    public static class Producer
    {
        public static void Produce(string eventMessage)
        {
            var factory = new ConnectionFactory { HostName = Names.HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Names.EventsExchange, ExchangeType.Fanout);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(Names.EventsExchange, string.Empty, properties, Encoding.UTF8.GetBytes(eventMessage));

            Console.WriteLine($"Sent event: {eventMessage}");
        }
    }
}
