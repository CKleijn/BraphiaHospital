using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public sealed record StaffMember
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid HospitalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmploymentDate { get; set; } = string.Empty;
    }
}