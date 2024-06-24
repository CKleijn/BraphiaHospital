using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event
{
    public sealed class StaffMemberCreatedEvent(StaffMember staffMember)
        : NotificationEvent, INotification
    {
        public StaffMember StaffMember { get; set; } = staffMember;
    }
}