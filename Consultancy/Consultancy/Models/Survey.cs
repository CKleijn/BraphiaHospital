namespace Consultancy.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Survey(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
