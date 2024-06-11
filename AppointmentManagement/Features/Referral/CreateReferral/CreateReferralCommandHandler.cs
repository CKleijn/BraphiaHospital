using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Infrastructure.Persistence;
using MediatR;
using System.Text.Json;

namespace AppointmentManagement.Features.Referral.CreateReferral
{
    public sealed class CreateReferralCommandHandler(IEventStore eventStore)
        : IRequestHandler<CreateReferralCommand>
    {
        public async Task Handle(
            CreateReferralCommand request,
            CancellationToken cancellationToken)
        {
            var result = await eventStore
                .AddEvent(
                EventKeys.REGISTER_KEY(Tags.APPOINTMENT_TAG),
                JsonSerializer.Serialize(request),
                cancellationToken);

            if (result)
            {
                //var newEvent = new PatientRegisteredEvent();
                // Do something
            }
        }
    }
}
