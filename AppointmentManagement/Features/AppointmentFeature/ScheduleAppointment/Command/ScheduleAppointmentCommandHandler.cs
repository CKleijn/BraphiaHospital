using AppointmentManagement.Common.Entities;
using AppointmentManagement.Common.Enums;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;
using System.Text.Json;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed class ScheduleAppointmentCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<ScheduleAppointmentCommand> validator,
        IAppointmentMapper mapper,
        ApplicationDbContext context,
        IApiClient apiClient,
        IConfiguration config)
        : IRequestHandler<ScheduleAppointmentCommand>
    {
        public async Task Handle(
            ScheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            /*            Patient? patient = await apiClient
                            .GetAsync<Patient>($"{ConfigurationHelper.GetPatientManagementServiceConnectionString()}/patient/{request.PatientId}", cancellationToken)
                            ?? throw new ArgumentNullException($"Patient #{request.PatientId} doesn't exist");*/

            //TODO: test patient when 
            var patient = new Patient();

            if (!await eventStore.EventByAggregateIdExists(request.ReferralId, cancellationToken))
                throw new ArgumentNullException($"Referral #{request.ReferralId} doesn't exist");

            if (!await eventStore.EventByAggregateIdExists(request.PhysicianId, cancellationToken))
                throw new ArgumentNullException($"Physician #{request.PhysicianId} doesn't exist");

            if (!await eventStore.EventByAggregateIdExists(request.HospitalFacilityId, cancellationToken))
                throw new ArgumentNullException($"HospitalFacility #{request.HospitalFacilityId} doesn't exist");

            Referral referral = new() { Id = request.ReferralId };
            StaffMember physician = new() { Id = request.PhysicianId };
            HospitalFacility hospitalFacility = new() { Id = request.HospitalFacilityId };

            Guid newId = Guid.NewGuid();

            AppointmentScheduledEvent appointmentScheduledEvent = new AppointmentScheduledEvent(
                newId,
                request.PatientId,
                request.ReferralId,
                request.PhysicianId,
                request.HospitalFacilityId,
                request.ScheduledDateTime,
                ArrivalStatus.Absent)
            {
                AggregateId = newId,
                Type = nameof(AppointmentScheduledEvent),
                Payload = JsonSerializer.Serialize(new
                {
                    Id = newId,
                    request.PatientId,
                    request.ReferralId,
                    request.PhysicianId,
                    request.HospitalFacilityId,
                    request.ScheduledDateTime,
                    Status = ArrivalStatus.Absent
                })
        };

            var result = await eventStore
                .AddEvent(
                appointmentScheduledEvent,
                    cancellationToken);

            if (!result)
                return;

            producer.Produce(
                EventMapper.MapEventToRoutingKey(appointmentScheduledEvent.GetType().Name),
                appointmentScheduledEvent.GetType().Name,
                JsonSerializer.Serialize(appointmentScheduledEvent));
        }
    }
}