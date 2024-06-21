using Consultancy.Common.Entities.DTO;
using MediatR;

namespace Consultancy.Features.ConsultFeature.CreateConsult.Command
{
    public sealed record CreateConsultCommand(
        Guid AppointmentId,
        string SurveyTitle,
        ICollection<QuestionDTO> Questions
        )
        : IRequest;
}
