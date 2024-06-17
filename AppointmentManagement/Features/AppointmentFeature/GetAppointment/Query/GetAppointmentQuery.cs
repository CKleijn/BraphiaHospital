using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointment.Query
{
    public sealed record GetAppointmentQuery(Guid Id)
        : IRequest<Appointment>;
}
