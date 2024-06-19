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
            if (await context.Set<Consult>().AnyAsync(c => c.AppointmentId == notification.AppointmentId, cancellationToken))
                throw new DuplicateNameException($"{notification.AppointmentId} already has a consult");

            if (await context.Set<Consult>().AnyAsync(c => c.Survey != null && notification.Survey != null && c.Survey.Id == notification.Survey.Id, cancellationToken))
                throw new DuplicateNameException($"Unable to create new survey with already existing survey id");

            var existingQuestionIds = await context.Set<Consult>()
                .Where(c => c.Survey != null)
                .SelectMany(c => c.Survey.Questions.Select(q => q.Id))
            .ToListAsync();

            if (notification.Survey != null && notification.Survey.Questions.Any(rq => existingQuestionIds.Contains(rq.Id)))
                throw new DuplicateNameException($"Unable to create new question with already existing question id");

            _ = await apiClient
                .GetAsync<Appointment>($"{ConfigurationHelper.GetAppointmentManagementServiceConnectionString()}/appointment/{notification.AppointmentId}", cancellationToken)
                ?? throw new ArgumentNullException($"Appointment #{notification.AppointmentId} doesn't exist");

            Consult consult = new()
            {
                Id = notification.Id,
                AppointmentId = notification.AppointmentId,
                Survey = notification.Survey
            };

            context.Set<Consult>().Add(consult);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
