using AppointmentManagement.Common.Aggregates;
using AppointmentManagement.Common.Enums;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentManagement.Common.Entities
{
    public class Appointment
        : AggregateRoot
    {
        public Guid PatientId { get; set; } = new Guid();
        [NotMapped]
        public Patient Patient { get; set; }
        public Guid ReferralId { get; set; } = new Guid();
        public Referral Referral { get; set; }
        public Guid PhysicianId { get; set; } = new Guid();
        public StaffMember Physician { get; set; }
        public Guid HospitalFacilityId { get; set; } = new Guid();
        public HospitalFacility HospitalFacility { get; set; }
        public ArrivalStatus Status { get; set; } = ArrivalStatus.Absent;
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;

        public void Apply(AppointmentScheduledEvent @event)
        {
            PatientId = @event.PatientId;
            ReferralId = @event.ReferralId;
            PhysicianId = @event.PhysicianId;
            HospitalFacilityId = @event.HospitalFacilityId;
            ScheduledDateTime = @event.ScheduledDateTime;
            Status = @event.Status;
        }

        public void Apply(AppointmentRescheduledEvent @event)
        {
            ScheduledDateTime = @event.ScheduledDateTime;
        }

        public void Apply(AppointmentArrivalUpdatedEvent @event)
        {
            Status = @event.Status;
        }
    }
}