using AppointmentManagement.Common.Abstractions;

namespace AppointmentManagement.Common.Helpers
{
    public static class Utils
    {
        public static int GetHighestVersionByType<T>(List<NotificationEvent> events) where T : NotificationEvent
        {
            return events.OfType<T>().Any() ? events.OfType<T>().Max(e => e.Version) : 0;
        }
    }
}
