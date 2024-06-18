namespace DossierManagement.Infrastructure.MessageBus
{
    public static class Keys
    {
        public static readonly string HOST_NAME = "rabbitmq";
        public static readonly int PORT_NAME = 5672;
        public static readonly string EVENTS_EXCHANGE = "EventsExchange";
        public static readonly string PATIENT_QUEUE_DOSSIERMANAGEMENT = "PatientQueue-DossierManagement";
        public static readonly string PATIENT_ROUTING_KEY = "Patient.#";
        public static readonly string DOSSIER_QUEUE_DOSSIERMANAGEMENT = "DossierQueue-DossierManagement";
        public static readonly string DOSSIER_ROUTING_KEY = "Dossier.#";
    }
}
