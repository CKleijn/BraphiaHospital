﻿using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Aggregates;
using AppointmentManagement.Common.Enums;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event;

namespace AppointmentManagement.Common.Entities
{
    public class Appointment
        : AggregateRoot
    {
        public Patient Patient { get; set; }
        public Referral Referral { get; set; }
        public StaffMember Physician { get; set; }
        public HospitalFacility HospitalFacility { get; set; }
        public ArrivalStatus Status { get; set; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;

        public void Apply(AppointmentScheduledEvent @event)
        {

            throw new System.NotImplementedException();

/*            Patient = @event.Patient;
            Referral = @event.Referral;
            Physician = @event.Appointment.Physician;
            HospitalFacility = @event.Appointment.HospitalFacility;
            ScheduledDateTime = @event.Appointment.ScheduledDateTime;
            Status = @event.Appointment.Status;*/
        }

        public void Apply(AppointmentRescheduledEvent @event)
        {
            ScheduledDateTime = @event.ScheduledDateTime;
        }

        public void Apply(PatientArrivalUpdatedEvent @event)
        {
            Status = @event.Status;
        }
    }
}