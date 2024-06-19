using MediatR;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Event
{
    public sealed class ConsultCreatedEventHandler(ApplicationDbContext context, IApiClient apiClient)
        : INotificationHandler<ConsultCreatedEvent>
    {
        public async Task Handle(
            ConsultCreatedEvent notification, 
            CancellationToken cancellationToken)
        {
            if (await context.Set<Consult>().AnyAsync(c => c.AppointmentId == notification.Consult.AppointmentId, cancellationToken))
                throw new DuplicateNameException($"{notification.Consult.AppointmentId} already has a consult");

            if (await context.Set<Consult>().AnyAsync(c => c.Survey != null && notification.Consult.Survey != null && c.Survey.Id == notification.Consult.Survey.Id, cancellationToken))
                throw new DuplicateNameException($"Unable to create new survey with already existing survey id");

            var existingQuestionIds = await context.Set<Consult>()
                .Where(c => c.Survey != null)
                .SelectMany(c => c.Survey.Questions.Select(q => q.Id))
            .ToListAsync();

            if (notification.Consult.Survey != null && notification.Consult.Survey.Questions.Any(rq => existingQuestionIds.Contains(rq.Id)))
                throw new DuplicateNameException($"Unable to create new question with already existing question id");

            _ = await apiClient
                .GetAsync<Appointment>($"{ConfigurationHelper.GetAppointmentManagementServiceConnectionString()}/appointment/{notification.Consult.AppointmentId}", cancellationToken)
                ?? throw new ArgumentNullException($"Appointment #{notification.Consult.AppointmentId} doesn't exist");

            context.Set<Consult>().Add(notification.Consult);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
