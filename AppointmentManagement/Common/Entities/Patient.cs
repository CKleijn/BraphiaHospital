﻿using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public string BSN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}