using Consultancy.Common.Entities;
using MediatR;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Command
{
    public sealed record CreateConsultCommand(
        Guid AppointmentId,
        Survey? Survey,
        string? Notes)
        : IRequest;
}
