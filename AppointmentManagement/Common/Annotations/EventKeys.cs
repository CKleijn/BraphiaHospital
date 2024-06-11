namespace AppointmentManagement.Common.Annotations
{
    public static class EventKeys
    {
        public static string REGISTER_KEY(string entity) => $"New {entity} registered";
    }
}
