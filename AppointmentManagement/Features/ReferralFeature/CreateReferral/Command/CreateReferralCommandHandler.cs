using FluentValidation;
using MediatR;
using System.Text.Json;
using AppointmentManagement.Common.Helpers;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;


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

            var referral = mapper.CreatedReferralCommandToReferral(request);

            var result = await eventStore
                .AddEvent(
                typeof(ReferralCreatedEvent).Name,
                JsonSerializer.Serialize(referral),
                cancellationToken);

            if (result)
            {
                var referralCreatedEvent = mapper.ReferralToReferralCreatedEvent(referral);

                producer.Produce(
                    EventMapper.MapEventToRoutingKey(referralCreatedEvent.GetType().Name),
                    referralCreatedEvent.GetType().Name,
                    JsonSerializer.Serialize(referralCreatedEvent));
            }
        }
    }
}
