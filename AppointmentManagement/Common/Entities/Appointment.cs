using AppointmentManagement.Common.Enums;
using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public sealed record Appointment
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Guid ReferralId { get; set; }
        public Guid PhysicianId { get; set; }
        public Guid HospitalFacilityId { get; set; }
        public ArrivalStatus Status { get; set; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;
    }
}