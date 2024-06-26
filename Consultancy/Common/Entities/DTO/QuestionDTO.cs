namespace Consultancy.Common.Entities.DTO
{
    public sealed record QuestionDTO
    {
        public string QuestionValue { get; set; } = string.Empty;
        public string? AnswerValue { get; set; } = string.Empty;
    }
}
