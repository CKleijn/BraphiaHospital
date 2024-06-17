namespace Consultancy.Infrastructure.MessageBus
{
    public static class Keys
    {
        public static readonly string HOST_NAME = "rabbitmq";
        public static readonly int PORT_NAME = 5672;
        public static readonly string EVENTS_EXCHANGE = "EventsExchange";
        public static readonly string CONSULT_ROUTING_KEY = "Consult.#";
        public static readonly string CONSULT_QUEUE = "ConsultQueue";
    }
}
