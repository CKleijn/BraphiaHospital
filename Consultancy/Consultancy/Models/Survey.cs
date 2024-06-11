namespace Consultancy.Models
{
    public class Survey(string id, string title)
    {
        public string Id { get; set; } = id;
        public string Title { get; set; } = title;
    }
}
