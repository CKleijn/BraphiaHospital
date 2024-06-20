using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event
{
    public sealed record StaffCreatedEvent(StaffMember StaffMember)
        : INotification;
}