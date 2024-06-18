namespace AppointmentManagement.Infrastructure.MessageBus
{
    public static class Keys
    {
        public static readonly string HOST_NAME = "rabbitmq";
        public static readonly int PORT_NAME = 5672;
        public static readonly string EVENTS_EXCHANGE = "EventsExchange";

        public static readonly string APPOINTMENT_ROUTING_KEY = "Appointment.#";
        public static readonly string REFERRAL_ROUTING_KEY = "Referral.#";
        public static readonly string PATIENT_ROUTING_KEY = "Patient.#";
        public static readonly string HOSPITALFACILITY_ROUTING_KEY = "HospitalFacility.#";
        public static readonly string STAFFMEMBER_ROUTING_KEY = "StaffMember.#";

        public static readonly string APPOINTMENT_APPOINTMENTMANAGEMENT_QUEUE = "AppointmentQueue-AppointmentManagement";
        public static readonly string REFERRAL_APPOINTMENTMANAGEMENT_QUEUE = "ReferralQueue-AppointmentManagement";
        public static readonly string PATIENT_APPOINTMENTMANAGEMENT_QUEUE = "PatientQueue-AppointmentManagement";
        public static readonly string HOSPITALFACILITY_APPOINTMENTMANAGEMENT_QUEUE = "HospitalFacilityQueue-AppointmentManagement";
        public static readonly string STAFFMEMBER_APPOINTMENTMANAGEMENT_QUEUE = "StaffMemberQueue-AppointmentManagement";

    }
}
