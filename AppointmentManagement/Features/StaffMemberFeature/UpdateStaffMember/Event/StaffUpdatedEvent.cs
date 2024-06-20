using AppointmentManagement.Common.Entities;
using MediatR;

namespace AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event
{
    public sealed record StaffUpdatedEvent(StaffMember StaffMember)
        : INotification;
}