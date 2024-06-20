using System.Text.RegularExpressions;

namespace Consultancy.Common.Helpers
{
    public static class EventMapper
    {
        public static string MapEventToRoutingKey(string eventName)
        {
            string[] wordsSplitted = Regex.Split(eventName, @"(?<!^)(?=[A-Z])");
            return string.Join(".", wordsSplitted);
        }
    }
}
