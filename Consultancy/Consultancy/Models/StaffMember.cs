namespace Consultancy.Models
{
    public class StaffMember
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StaffMember(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
