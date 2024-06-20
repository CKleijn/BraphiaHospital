using FluentValidation;

namespace Consultancy.Features.ConsultFeature.UpdateNotes.Command
{
    public sealed class UpdateNotesCommandValidator
        : AbstractValidator<UpdateNotesCommand>
    {
        public UpdateNotesCommandValidator()
        {
            RuleFor(n => n.Notes)
                .NotEmpty()
                .WithMessage("Notes are required!");
        }
    }
}
