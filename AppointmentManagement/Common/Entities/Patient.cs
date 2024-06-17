﻿using AppointmentManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Common.Entities
{
    public class Patient
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public string BSN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}