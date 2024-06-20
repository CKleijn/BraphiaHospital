using MediatR;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Command
{
    public sealed record UpdateNotesCommand(
        Guid Id,
        string Notes
    ) : IRequest
    {
        public Guid Id { get; set; }
    }
}
