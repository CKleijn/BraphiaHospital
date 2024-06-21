using Consultancy.Common.Enums;
using Consultancy.Common.Interfaces;

namespace Consultancy.Common.Entities
{
    public sealed record Appointment
        : IEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid PatientId { get; set; } = Guid.Empty;
        public Guid ReferralId { get; set; } = Guid.Empty;
        public Guid PhysicianId { get; set; } = Guid.Empty;
        public Guid HospitalFacilityId { get; set; } = Guid.Empty;
        public ArrivalStatus Status { get; set; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;
    }
}