using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PatientManagement.Infrastructure.MessageBus
{
    public static class Consumer
    {
        public static void Consume()
        {
            var factory = new ConnectionFactory { HostName = Names.HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Names.EventsExchange, ExchangeType.Fanout);

            channel.QueueDeclare(Names.PatientQueue, true, false, false, null);

            channel.QueueBind(Names.PatientQueue, Names.EventsExchange, string.Empty);

            channel.BasicQos(0, 1, false);

            Console.WriteLine("Waiting for events...");

            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var eventMessage = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received event: {eventMessage}");

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            
            channel.BasicConsume(Names.PatientQueue, false, consumer);

            Console.ReadLine();
        }
    }
}
