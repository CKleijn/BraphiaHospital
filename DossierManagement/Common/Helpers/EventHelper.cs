using System.Text.RegularExpressions;

namespace DossierManagement.Common.Helpers
{
    public static class EventHelper
    {
        public static string MapEventToRoutingKey(string eventName)
        {
            string[] wordsSplitted = Regex.Split(eventName, @"(?<!^)(?=[A-Z])");
            return string.Join(".", wordsSplitted);
        }
    }
}
