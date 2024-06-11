namespace Consultancy.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public Question(int id, int surveyId, string questionText, string answerText)
        {
            Id = id;
            SurveyId = surveyId;
            QuestionText = questionText;
            AnswerText = answerText;
        }
    }
}
