using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command
{
    public sealed class UpdatePatientArrivalCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<UpdatePatientArrivalCommand> validator,
        IReferralMapper mapper)
        : IRequestHandler<UpdatePatientArrivalCommand>
    {
        public async Task Handle(
            UpdatePatientArrivalCommand request,
            CancellationToken cancellationToken)
        {

            //TODO: IMPLEMENT

        }
    }
}
