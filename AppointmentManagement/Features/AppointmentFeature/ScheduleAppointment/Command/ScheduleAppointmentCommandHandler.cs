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
        ApplicationDbContext context)
        : IRequestHandler<ScheduleAppointmentCommand>
    {
        public async Task Handle(
            ScheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {

            //TODO: if partial payload is allowed there will be no need for the context call
            //Move id validation to validator when partial payload will be implemented

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //TODO: patient check through api for most recent data, cause its important a patient is able to create an appointment instantly
            _ = await context.Set<Patient>()
                .FindAsync(request.PatientId, cancellationToken) ?? throw new ArgumentNullException($"Patient #{request.PatientId} doesn't exist");

            _ = await context.Set<Referral>()
                .FindAsync(request.ReferralId, cancellationToken) ?? throw new ArgumentNullException($"Referral #{request.ReferralId} doesn't exist");

            _ = await context.Set<StaffMember>()
                .FindAsync(request.PhysicianId, cancellationToken) ?? throw new ArgumentNullException($"Physician #{request.PhysicianId} doesn't exist");

            _ = await context.Set<HospitalFacility>()
                .FindAsync(request.HospitalFacilityId, cancellationToken) ?? throw new ArgumentNullException($"HospitalFacility #{request.HospitalFacilityId} doesn't exist");
            
            AppointmentScheduledEvent appointmentScheduledEvent = new AppointmentScheduledEvent(
                Id: Guid.NewGuid(),
                PatientId: request.PatientId,
                ReferralId: request.ReferralId,
                PhysicianId: request.PhysicianId,
                HospitalFacilityId: request.HospitalFacilityId,
                Status: ArrivalStatus.Absent,
                ScheduledDateTime: request.ScheduledDateTime
            );

            var result = await eventStore
                .AddEvent(
                    typeof(AppointmentScheduledEvent).Name,
                    JsonSerializer.Serialize(appointmentScheduledEvent),
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