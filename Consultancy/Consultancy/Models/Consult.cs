namespace Consultancy.Models
{
    public class Consult(string id, string appointmentId, string surveyId)
    {
        public string Id { get; set; } = id;
        public string AppointmentId { get; set; } = appointmentId;
        public string SurveyId { get; set; } = surveyId;
    }
}
