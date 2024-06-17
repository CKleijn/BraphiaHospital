using FluentValidation;
using MediatR;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;

namespace AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command
{
    public sealed class RescheduleAppointmentCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<RescheduleAppointmentCommand> validator,
        IReferralMapper mapper)
        : IRequestHandler<RescheduleAppointmentCommand>
    {
        public async Task Handle(
            RescheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {

            //TODO: IMPLEMENT

        }
    }
}