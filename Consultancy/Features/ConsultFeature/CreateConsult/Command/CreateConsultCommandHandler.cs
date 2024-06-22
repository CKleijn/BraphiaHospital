using FluentValidation;
using MediatR;
using Consultancy.Common.Helpers;

using System.Data;
using System.Text.Json;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Entities;
using Consultancy.Common.Interfaces;
using Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Command
{
    public sealed class CreateConsultCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<CreateConsultCommand> validator,
        IApiClient apiClient,
        ApplicationDbContext context)
        IQuestionMapper mapper)
        : IRequestHandler<CreateConsultCommand>
    {
        public async Task Handle(
            CreateConsultCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (await eventStore.AppointmentAlreadyHasConsult(request.AppointmentId, cancellationToken))
                throw new DuplicateNameException($"{request.AppointmentId} already has a consult");

            Appointment coupledAppointment = await apiClient
                .GetAsync<Appointment>($"{ConfigurationHelper.GetAppointmentManagementServiceConnectionString()}/appointment/{request.AppointmentId}", cancellationToken)
                ?? throw new ArgumentNullException($"Appointment #{request.AppointmentId} doesn't exist");

            Survey survey = null!;
            if (request.SurveyTitle != null)
                survey = new()
                {
                    Title = request.SurveyTitle,
                    Questions = mapper.QuestionsDTOToQuestions(request.Questions),
                };

            Consult consult = new()
            {
                PatientId = coupledAppointment.PatientId,
                AppointmentId = coupledAppointment.Id,
                Survey = survey
            };

            ConsultCreatedEvent consultCreatedEvent = new (consult)
            {
                AggregateId = consult.Id,
                Type = nameof(ConsultCreatedEvent),
                Payload = JsonSerializer.Serialize(consult),
                Version = 0
            };

            var result = await eventStore
                .AddEvent(
                    consultCreatedEvent,
                    cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventHelper.MapEventToRoutingKey(consultCreatedEvent.GetType().Name),
                consultCreatedEvent.GetType().Name,
                JsonSerializer.Serialize(consultCreatedEvent));
        }
    }
}
