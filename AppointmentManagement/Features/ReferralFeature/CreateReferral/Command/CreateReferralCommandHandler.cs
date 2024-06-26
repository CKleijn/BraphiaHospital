using FluentValidation;
using MediatR;
using System.Text.Json;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;
using AppointmentManagement.Common.Entities;


namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Command
{
    public sealed class CreateReferralCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<CreateReferralCommand> validator,
        IReferralMapper mapper)
        : IRequestHandler<CreateReferralCommand>
    {
        public async Task Handle(
            CreateReferralCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!await eventStore.EventByAggregateIdExists(request.HospitalFacilityId, cancellationToken))
                throw new ArgumentNullException($"Hospital #{request.HospitalFacilityId} doesn't exist");

            Referral referral = new Referral
            {
                Diagnosis = request.Diagnosis,
                BSN = request.BSN,
                HospitalFacilityId = request.HospitalFacilityId,
            };

            ReferralCreatedEvent referralCreatedEvent = new(referral)
            {
                AggregateId = referral.Id,
                Type = nameof(ReferralCreatedEvent),
                Payload = JsonSerializer.Serialize(referral),
            };

            bool result = await eventStore
                .AddEvent(
                referralCreatedEvent,
                cancellationToken);

            if (result)
            {
                producer.Produce(
                    EventMapper.MapEventToRoutingKey(referralCreatedEvent.GetType().Name),
                    referralCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(referralCreatedEvent));
            }
        }
    }
}
