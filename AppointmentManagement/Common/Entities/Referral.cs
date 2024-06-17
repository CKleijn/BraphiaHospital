using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public sealed record Referral
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid HospitalFacilityId { get; init; }
        public string ReferralCode { get; init; } = Guid.NewGuid().ToString();
        public string BSN { get; init; } = string.Empty;
        public string Diagnosis { get; init; } = string.Empty;
    }
}