using MediatR;

namespace Consultancy.Features.Consult.RegisterConsult.Command
{
    public sealed record RegisterConsultCommand(
        Guid AppointmentId,
        Guid DossierId,
        Guid SurveyId)
        : IRequest;
}
