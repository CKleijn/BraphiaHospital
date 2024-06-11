namespace Consultancy.Models
{
    public class Consult
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int SurveyId { get; set; }

        public Consult(int id, int appointmentId, int surveyId)
        {
            Id = id;
            AppointmentId = appointmentId;
            SurveyId = surveyId;
        }
    }
}
