using AppointmentManagement.Common.Aggregates;
using AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event;
using AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event;

namespace AppointmentManagement.Common.Entities
{
    public class StaffMember
        : AggregateRoot
    {
        public Guid HospitalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public void Apply(StaffCreatedEvent @event)
        {
            HospitalId = @event.StaffMember.HospitalId;
            Name = @event.StaffMember.Name;
            Specialization = @event.StaffMember.Specialization;
        }

        public void Apply(StaffUpdatedEvent @event)
        {
            HospitalId = @event.HospitalId;
            Name = @event.Name;
            Specialization = @event.Specialization;
        }
    }
}