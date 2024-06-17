namespace AppointmentManagement.Infrastructure.MessageBus
{
    public static class Keys
    {
        public static readonly string HOST_NAME = "rabbitmq";
        public static readonly int PORT_NAME = 5672;
        public static readonly string EVENTS_EXCHANGE = "EventsExchange";
        public static readonly string APPOINTMENT_QUEUE = "AppointmentQueue";
        public static readonly string APPOINTMENT_ROUTING_KEY = "Appointment.#";
    }
}
