using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using FluentValidation;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command
{
    public sealed class ScheduleAppointmentCommandHandler(
        IProducer producer,
        IEventStore eventStore,
        IValidator<ScheduleAppointmentCommand> validator,
        IAppointmentMapper mapper)
        : IRequestHandler<ScheduleAppointmentCommand>
    {
        public async Task Handle(
            ScheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {

            //TODO: IMPLEMENT

        }
    }
}