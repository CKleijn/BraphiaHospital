using AppointmentManagement.Common.Abstractions;
using MediatR;

namespace AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event
{
    public sealed class StaffUpdatedEvent(Guid id, Guid hospitalId, string name, string specialization)
        : NotificationEvent, INotification
    {
        public Guid Id { get; set; } = id;
        public Guid HospitalId { get; set; } = hospitalId;
        public string Name { get; set; } = name;
        public string Specialization { get; set; } = specialization;
    }
}