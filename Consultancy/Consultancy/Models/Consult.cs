namespace Consultancy.Models
{
    public class Consult
    {
        public int Id { get; set; }
        public Survey Survey { get; set; }
        public Appointment Appointment { get; set; }

        public Consult(int id, Survey survey, Appointment appointment)
        {
            Id = id;
            Survey = survey;
            Appointment = appointment;
        }
    }
}
