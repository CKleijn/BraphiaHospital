using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Features.Consult
{
    public sealed record Consult
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid AppointmentId { get; set; } = Guid.Empty;
        public Survey Survey { get; set; } = null;
    }
}
