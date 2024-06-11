namespace Consultancy.Models
{
    public class Question(string id, string surveyId, string questionValue, string answerValue)
    {
        public string Id { get; set; } = id;
        public string SurveyId { get; set; } = surveyId;
        public string QuestionValue { get; set; } = questionValue;
        public string AnswerValue { get; set; } = answerValue;
    }
}
