namespace Consultancy.Models
{
    public class Consultancy
    {
        public int Id { get; set; }
        public Survey Survey { get; set; }
        public Appointment Appointment { get; set; }

        public Consultancy(int id, Survey survey, Appointment appointment)
        {
            Id = id;
            Survey = survey;
            Appointment = appointment;
        }
    }
}
