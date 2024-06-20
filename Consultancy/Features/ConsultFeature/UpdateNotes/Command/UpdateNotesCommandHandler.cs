using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Consultancy.Common.Helpers;

using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.UpdateNotes.Event;
using Microsoft.IdentityModel.Tokens;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Command
{
    public sealed class UpdateNotesCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdateNotesCommand> validator,
        IApiClient apiClient,
        ApplicationDbContext context)
        : IRequestHandler<UpdateNotesCommand>
    {
        public async Task Handle(
            UpdateNotesCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!await context.Set<Consult>().AnyAsync(c => c.Id == request.Id, cancellationToken))
                throw new KeyNotFoundException($"No consult present with id #{request.Id}");

            if (await context.Set<Consult>().AnyAsync(c => c.Id == request.Id && c.Notes.IsNullOrEmpty()!, cancellationToken))
                throw new InvalidOperationException($"Consult with id #{request.Id} has already finished and therefore cannot be edited");

            Consult consult = await context.FindAsync<Consult>(request.Id) ?? throw new KeyNotFoundException($"No consult present with id #{request.Id}");

            Appointment coupledAppointment = await apiClient
               .GetAsync<Appointment>($"{ConfigurationHelper.GetAppointmentManagementServiceConnectionString()}/appointment/{consult.AppointmentId}", cancellationToken)
               ?? throw new KeyNotFoundException($"Appointment #{consult.AppointmentId} doesn't exist");

            consult.Notes = request.Notes;

            DossierConsultAppendedEvent dossierConsultAppendedEvent = new DossierConsultAppendedEvent(
                PatientId: coupledAppointment.PatientId,
                Consult: consult
            );

            var result = await eventStore
                .AddEvent(
                    typeof(DossierConsultAppendedEvent).Name,
                    JsonSerializer.Serialize(dossierConsultAppendedEvent),
                    cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventMapper.MapEventToRoutingKey(dossierConsultAppendedEvent.GetType().Name),
                dossierConsultAppendedEvent.GetType().Name,
                JsonSerializer.Serialize(dossierConsultAppendedEvent));
        }
    }
}
