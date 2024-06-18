using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Features.Consult
{
    public sealed record Survey
    : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public ICollection<Consult> Consults { get; set; } = [];
        public ICollection<Question> Questions { get; set; } = [];
    }
}
