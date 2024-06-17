using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public sealed record Referral
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Diagnosis { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
    }
}