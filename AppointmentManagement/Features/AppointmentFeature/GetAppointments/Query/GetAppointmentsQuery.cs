using MediatR;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointments.Query
{
    public sealed record GetAppointmentsQuery()
        : IRequest<IEnumerable<Appointment>>;
}
