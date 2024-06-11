namespace Consultancy.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Patient Patient { get; set; }
        public StaffMember StaffMember { get; set; }

        public Appointment(int id, DateTime date, Patient patient, StaffMember staffMember)
        {
            Id = id;
            Date = date;
            Patient = patient;
            StaffMember = staffMember;
        }
    }
}
