using MediatR;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;
using System.Text.Json;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event
{
    public sealed class StaffUpdatedEventHandler(
        ApplicationDbContext context,
        IEventStore eventStore)
        : INotificationHandler<StaffUpdatedEvent>
    {
        public async Task Handle(
            StaffUpdatedEvent notification,
            CancellationToken cancellationToken)
        {

            StaffMember? staffToUpdate = await context.Set<StaffMember>()
                .FindAsync(notification.StaffMember.Id, cancellationToken);

            var result = await eventStore
                .AddEvent(
                    typeof(StaffUpdatedEvent).Name,
                    JsonSerializer.Serialize(notification.StaffMember),
                    cancellationToken);

            if (!result)
                return;

            // update through event sourcing

            staffToUpdate.Name = notification.StaffMember.Name;
            staffToUpdate.Specialization = notification.StaffMember.Specialization;
            staffToUpdate.Street = notification.StaffMember.Street;
            staffToUpdate.City = notification.StaffMember.City;
            staffToUpdate.State = notification.StaffMember.State;
            staffToUpdate.Zip = notification.StaffMember.Zip;
            staffToUpdate.PhoneNumber = notification.StaffMember.PhoneNumber;
            staffToUpdate.Email = notification.StaffMember.Email;
            staffToUpdate.EmploymentDate = notification.StaffMember.EmploymentDate;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}