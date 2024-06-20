using AppointmentManagement.Common.Enums;
using MediatR;

namespace AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command
{
    public sealed record UpdatePatientArrivalCommand(
        Guid Id,
        ArrivalStatus Status
    ) : IRequest
    {
        public Guid Id { get; set;}
    }
}
