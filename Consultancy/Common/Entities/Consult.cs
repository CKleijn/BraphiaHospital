using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Common.Entities
{
    public sealed record Consult
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid AppointmentId { get; set; } = Guid.Empty;
        public Survey? Survey { get; set; } = null;
        public string? Notes { get; set; } = string.Empty;
    }
}
