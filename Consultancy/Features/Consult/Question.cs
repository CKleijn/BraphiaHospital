using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Features.Consult
{
    public sealed record Question
    : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public required Survey Survey { get; set; }
        public string QuestionValue { get; set; } = string.Empty;
        public string AnswerValue { get; set; } = string.Empty;
    }
}
