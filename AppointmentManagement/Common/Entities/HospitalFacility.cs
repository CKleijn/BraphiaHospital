using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public sealed record HospitalFacility
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Hospital { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Stores { get; set; } = 0;
        public int Squares { get; set; } = 0;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public int TotalBeds { get; set; } = 0;
        public int BuiltYear { get; set; } = 0;
    }
}
