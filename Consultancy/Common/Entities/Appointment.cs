using Consultancy.Common.Enums;
using Consultancy.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Consultancy.Common.Entities
{
    public sealed record Appointment
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        public Guid PatientId { get; set; }
        public Guid Referral { get; set; }
        public Guid Physician { get; set; }
        public Guid HospitalFacility { get; set; }

        public ArrivalStatus Status { get; set; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;
    }
}