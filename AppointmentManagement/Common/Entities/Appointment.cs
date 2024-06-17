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

        public Patient Patient { get; set; }
        public Referral Referral { get; set; }
        public StaffMember Physician { get; set; }
        public HospitalFacility HospitalFacility { get; set; }

        public ArrivalStatus Status { get; init; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; init; } = DateTime.Now;
    }
}