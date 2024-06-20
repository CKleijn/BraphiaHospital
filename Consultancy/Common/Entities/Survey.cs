using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Common.Entities
{
    public sealed record Survey
    : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public ICollection<Question> Questions { get; set; } = [];
    }
}
