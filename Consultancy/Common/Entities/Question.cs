using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Common.Entities
{
    public sealed record Question
    : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string QuestionValue { get; set; } = string.Empty;
        public string? AnswerValue { get; set; } = string.Empty;
    }
}
