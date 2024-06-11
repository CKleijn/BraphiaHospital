namespace Consultancy.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> Content { get; set; }

        public Survey(int id, string title, Dictionary<string, string> content)
        {
            Id = id;
            Title = title;
            Content = content;
        }
    }
}
