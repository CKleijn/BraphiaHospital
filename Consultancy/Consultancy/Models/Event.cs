namespace Consultancy.Models
{
    public class Event(string id, string eventValue)
    {
        public string Id { get; set; } = id;
        public string EventValue { get; set; } = eventValue;
    }
}
